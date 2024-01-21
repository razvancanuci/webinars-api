using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandler : RequestHandlerBase, IQueryHandler<AvailableWebinarByIdRequest>
{
    private readonly ICacheService _cacheService;
    private readonly IFileStorage _fileStorage;

    public GetAvailableWebinarByIdQueryHandler(ICacheService cacheService, IFileStorage fileStorage, IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
        _cacheService = cacheService;
        _fileStorage = fileStorage;
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
        
        var image = await _fileStorage.GetAsync(result.Id);
        
        var mappedResult = result.Adapt<WebinarInfoDto>();
        mappedResult.ImageUri = image;

        return new OkObjectResult(mappedResult);
    }
}