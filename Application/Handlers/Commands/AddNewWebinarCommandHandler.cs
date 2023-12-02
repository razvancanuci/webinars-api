using Application.Requests;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class AddNewWebinarCommandHandler : RequestHandlerBase, IRequestHandler<NewWebinarRequest, IActionResult>
{
    public AddNewWebinarCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    
    public async Task<IActionResult> Handle(NewWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinar = new Webinar
        {
            Title = request.Title,
            Description = request.Description,
            Host = request.Host,
            ScheduleDate = request.DateScheduled
        };
        await UnitOfWork.WebinarRepository.InsertAsync(webinar);
        await UnitOfWork.SaveAsync();
        return new CreatedResult("AddWebinar", request);
    }
}