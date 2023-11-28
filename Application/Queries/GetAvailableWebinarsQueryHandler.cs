using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Queries;

public class GetAvailableWebinarsQueryHandler : IRequestHandler<AvailableWebinarsRequest, IActionResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAvailableWebinarsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IActionResult> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.WebinarRepository
            .GetAsync(entity => entity.ScheduleDate > DateTime.UtcNow.AddDays(-2),
                query => query.Skip((request.Page - 1) * request.ItemsPerPage).Take(request.ItemsPerPage),
                asNoTracking: true);
        
        return  new OkObjectResult(result);
    }
}