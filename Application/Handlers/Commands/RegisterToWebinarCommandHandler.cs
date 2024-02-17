using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Messages;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Commands;

public class RegisterToWebinarCommandHandler : RequestHandlerBase, ICommandHandler<RegisterWebinarRequest>
{
    private readonly IMessageService _messageService;

    public RegisterToWebinarCommandHandler(IMessageService messageService, IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        _messageService = messageService;
    }
    
    public async Task<IResult> Handle(RegisterWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinarList = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.Id == request.WebinarId);
        
        var webinar = webinarList.FirstOrDefault();
        
        if (webinar is null)
        {
            return Results.NotFound("The id was not found in the database");
        }
        
        webinar.PeopleRegistered.Add(request.Person);
        await UnitOfWork.SaveAsync();


        var message = new EmailRegistrationMessage(
            request.Person.Name,
                request.Person.Email,
                webinar.Title,
                webinar.Host);

        await SendEmail(message);
        
        return Results.NoContent();
    }

    private async Task SendEmail(EmailRegistrationMessage message)
    {
        await _messageService.Send(message);
    }
}
