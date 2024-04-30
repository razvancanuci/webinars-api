using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarByIdQueryHandler : RequestHandlerBase, IQueryHandler<AvailableWebinarByIdRequest>
{
    private readonly ICacheService _cacheService;
    public GetAvailableWebinarByIdQueryHandler(ICacheService cacheService, IFileStorage fileStorage, IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
        _cacheService = cacheService;
    }
    public async Task<IResult> Handle(AvailableWebinarByIdRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var webinar = await _cacheService.GetOrCreateAsync(request.Key,
            () => UnitOfWork.WebinarRepository
                .GetByIdAsync(request.WebinarId),
            request.Expiration,
            cancellationToken);
        
        if (webinar is null)
        {
            return Results.NotFound("The id was not found");
        }
        
        var mappedResult = webinar.Adapt<WebinarInfoDto>();

        return Results.Ok(mappedResult);
    }
}