using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Queries;

public class GetAvailableWebinarByIdQueryHandler : IRequestHandler<AvailableWebinarByIdRequest, IActionResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAvailableWebinarByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IActionResult> Handle(AvailableWebinarByIdRequest request, CancellationToken cancellationToken)
    {
        var webinars = await _unitOfWork.WebinarRepository
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