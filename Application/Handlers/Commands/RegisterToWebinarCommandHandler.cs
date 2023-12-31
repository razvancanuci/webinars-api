using Application.Requests;
using Domain.Dtos;
using Domain.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class RegisterToWebinarCommandHandler : RequestHandlerBase, IRequestHandler<RegisterWebinarRequest, IActionResult>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterToWebinarCommandHandler(IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        _publishEndpoint = publishEndpoint;
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

        var sendEmailDto = new SendEmailDto(
            request.Person.Name,
            request.Person.Email,
            webinar.Title,
            webinar.Host);

        await SendEmail(sendEmailDto);
        
        return new NoContentResult();
    }

    private async Task SendEmail(SendEmailDto emailDto)
    {
        await _publishEndpoint.Publish(emailDto);
    }
}
