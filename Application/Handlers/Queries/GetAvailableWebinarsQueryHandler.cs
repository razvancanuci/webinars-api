using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarsQueryHandler : RequestHandlerBase, IRequestHandler<AvailableWebinarsRequest, IActionResult>
{
    public GetAvailableWebinarsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<IActionResult> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.ScheduleDate > DateTime.UtcNow.AddDays(-2),
                query => query.Skip((request.Page - 1) * request.ItemsPerPage).Take(request.ItemsPerPage),
                asNoTracking: true);
        
        return  new OkObjectResult(result);
    }
}