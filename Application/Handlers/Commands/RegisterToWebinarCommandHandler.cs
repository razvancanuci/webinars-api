﻿using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Dtos;
using Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class RegisterToWebinarCommandHandler : RequestHandlerBase, ICommandHandler<RegisterWebinarRequest>
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


        var message = new EmailRegistrationMessage(
            request.Person.Name,
                request.Person.Email,
                webinar.Title,
                webinar.Host);

        await SendEmail(message);
        
        return new NoContentResult();
    }

    private async Task SendEmail(EmailRegistrationMessage emailDto)
    {
        await _publishEndpoint.Publish(emailDto);
    }
}
