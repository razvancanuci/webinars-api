using Application.Requests;
using Application.Services.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandler : RequestHandlerBase, IRequestHandler<AvailableWebinarByIdRequest, IActionResult>
{
    private readonly ICacheService _cacheService;

    public GetAvailableWebinarByIdQueryHandler(ICacheService cacheService, IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
        _cacheService = cacheService;
    }
    public async Task<IActionResult> Handle(AvailableWebinarByIdRequest request, CancellationToken cancellationToken)
    {
        var webinars = await _cacheService.GetOrCreateAsync(request.Key,
            () => UnitOfWork.WebinarRepository
                .GetAsync(entity => entity.Id == request.WebinarId,
                    additionalQuery: query => query.Select(w => new Webinar
                    {
                       Id = w.Id,
                       Description = w.Description,
                       Host = w.Host,
                       Title = w.Title,
                       ScheduleDate = w.ScheduleDate
                    }),
                    asNoTracking: true),
            request.Expiration);
        
        var result = webinars.FirstOrDefault();
        
        if (result is null)
        {
            return new NotFoundObjectResult("The id was not found");
        }

        if (!result.IsAvailable())
        {
            return new BadRequestObjectResult("The webinar registrations were finished");
        }

        var mappedResult = result.Adapt<WebinarInfoDto>();

        return new OkObjectResult(mappedResult);
    }
}