using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Entities;
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
        var webinar = await UnitOfWork.WebinarRepository
            .GetByIdAsync(request.WebinarId);
        
        if (webinar is null)
        {
            return Results.NotFound("The id was not found in the database");
        }
        
        cancellationToken.ThrowIfCancellationRequested();

        var person = new Person
        {
            Name = request.Person.Name,
            Email = request.Person.Email,
            PhoneNumber = request.Person.PhoneNumber,
            WebinarId = webinar.Id
        };
        webinar.PeopleRegistered.Add(person);
        await UnitOfWork.SaveAsync(cancellationToken);
        
        var message = new EmailRegistrationMessage(
            request.Person.Name,
                request.Person.Email,
                webinar.Title,
                webinar.Host);

        await _messageService.Send(message, cancellationToken);
        
        return Results.NoContent();
    }
}
