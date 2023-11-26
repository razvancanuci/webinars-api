using Application.Requests;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class AddNewWebinarCommandHandler : IRequestHandler<NewWebinarRequest, IActionResult>
{
    private readonly IRepository _repository;
    
    public AddNewWebinarCommandHandler(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IActionResult> Handle(NewWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinar = new Webinar
        {
            Title = request.Title,
            Description = request.Description,
            Host = request.Host,
            ScheduleDate = request.DateScheduled
        };
        await _repository.AddWebinarAsync(webinar);
        return new CreatedResult("AddWebinar", request);
    }
}