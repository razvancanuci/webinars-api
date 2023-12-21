using Application.Requests;
using Domain.Dtos;
using Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarsQueryHandler : RequestHandlerBase, IRequestHandler<AvailableWebinarsRequest, IActionResult>
{
    public GetAvailableWebinarsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<IActionResult> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        var dateCompared = DateTime.UtcNow.AddDays(-2);
        var result = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.ScheduleDate > dateCompared,
                query => query.OrderBy(x => x.ScheduleDate).Skip((request.Page - 1) * request.ItemsPerPage).Take(request.ItemsPerPage),
                asNoTracking: true);
        var mappedResult = result.Adapt<IEnumerable<WebinarShortInfoDto>>();
        
        return  new OkObjectResult(mappedResult);
    }
}