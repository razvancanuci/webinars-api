using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Messages;
using Microsoft.AspNetCore.Http;

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
    
    public async Task<IResult> Handle(CancelWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinar = await UnitOfWork.WebinarRepository.GetByIdAsync(request.WebinarId);

        if (webinar is null)
        {
            return Results.NotFound("The webinar with specified id is not available");
        }
        
        cancellationToken.ThrowIfCancellationRequested();

        await DeleteWebinar(webinar, request.KeyToDelete, cancellationToken);
        
        if (webinar.PeopleRegistered.Count > 0)
        {
            await SendEmail(webinar.PeopleRegistered);
        }
        
        return Results.NoContent();
    }

    private async Task SendEmail(IEnumerable<Person> people)
    {
        var message = new EmailCancellationMessage(people);

        await _messageService.Send(message);
    }

    private async Task DeleteWebinar(Webinar webinar, string cacheKey, CancellationToken cancellationToken)
    {
        await _cacheService.DeleteKeyAsync(cacheKey, cancellationToken: cancellationToken);
        UnitOfWork.WebinarRepository.Delete(webinar);
        await UnitOfWork.SaveAsync(cancellationToken);
    }
}