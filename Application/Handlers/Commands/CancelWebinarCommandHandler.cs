using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class CancelWebinarCommandHandler : RequestHandlerBase, ICommandHandler<CancelWebinarRequest>
{
    private readonly ICacheService _cacheService;
    private readonly IMessageService _messageService;
    public CancelWebinarCommandHandler(
        ICacheService cacheService,
        IMessageService messageService,
        IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _cacheService = cacheService;
        _messageService = messageService;
    }
    
    public async Task<IActionResult> Handle(CancelWebinarRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.WebinarRepository.GetAsync(
            criteria: w => w.Id == request.WebinarId);

        var webinar = queryResult.FirstOrDefault();

        if (webinar is null)
        {
            return new NotFoundObjectResult("The webinar with specified id is not available");
        }
        
        await _cacheService.DeleteKeyAsync(request.KeyToDelete);
        
        UnitOfWork.WebinarRepository.Delete(webinar);
        await UnitOfWork.SaveAsync();

        if (webinar.PeopleRegistered.Count > 0)
        {
            await SendEmail(webinar.PeopleRegistered);
        }
        
        return new NoContentResult();
    }

    private async Task SendEmail(IEnumerable<Person> people)
    {
        var message = new EmailCancellationMessage(people);

        await _messageService.Send(message);
    }
}