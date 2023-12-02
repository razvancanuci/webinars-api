using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandler : RequestHandlerBase, IRequestHandler<AvailableWebinarByIdRequest, IActionResult>
{
    public GetAvailableWebinarByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    public async Task<IActionResult> Handle(AvailableWebinarByIdRequest request, CancellationToken cancellationToken)
    {
        var webinars = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.Id == request.WebinarId, asNoTracking: true);

        var result = webinars.FirstOrDefault();
        
        if (result is null)
        {
            return new NotFoundObjectResult("The id was not found");
        }

        if (result.ScheduleDate <= DateTime.UtcNow.AddDays(-2))
        {
            return new BadRequestObjectResult("The webinar registrations were finished");
        }

        return new OkObjectResult(result);
    }
}