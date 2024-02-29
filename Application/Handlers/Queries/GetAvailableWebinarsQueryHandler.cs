using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Constants;
using Domain.Dtos;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Specifications.Webinar;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarsQueryHandler : RequestHandlerBase, IQueryHandler<AvailableWebinarsRequest>
{
    public GetAvailableWebinarsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<IResult> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.WebinarRepository
            .GetAsync(new GetWebinarsPaginatedSpecification(request));
        
        var mappedResult = result.Adapt<IEnumerable<WebinarShortInfoDto>>();
        
        return Results.Ok(mappedResult);
    }
}