using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class RegisterToWebinarCommandHandler : RequestHandlerBase, ICommandHandler<RegisterWebinarRequest>
{
    private readonly IMessageService _messageService;

    public RegisterToWebinarCommandHandler(IMessageService messageService, IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        _messageService = messageService;
    }
    
    public async Task<IActionResult> Handle(RegisterWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinarList = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.Id == request.WebinarId);
        
        var webinar = webinarList.FirstOrDefault();
        
        if (webinar is null)
        {
            return new NotFoundResult();
        }
        
        webinar.PeopleRegistered.Add(request.Person);
        await UnitOfWork.SaveAsync();


        var message = new EmailRegistrationMessage(
            request.Person.Name,
                request.Person.Email,
                webinar.Title,
                webinar.Host);

        await SendEmail(message);
        
        return new NoContentResult();
    }

    private async Task SendEmail(EmailRegistrationMessage message)
    {
        await _messageService.Send(message);
    }
}
