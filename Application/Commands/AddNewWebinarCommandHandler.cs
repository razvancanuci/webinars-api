using Application.Requests;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class AddNewWebinarCommandHandler : IRequestHandler<NewWebinarRequest, IActionResult>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AddNewWebinarCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.WebinarRepository.InsertAsync(webinar);
        await _unitOfWork.SaveAsync();
        return new CreatedResult("AddWebinar", request);
    }
}