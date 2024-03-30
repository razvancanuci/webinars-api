using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Dtos;
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
        cancellationToken.ThrowIfCancellationRequested();
        
        var webinars = await UnitOfWork.WebinarRepository
            .GetAsync(new GetWebinarsPaginatedSpecification(request), cancellationToken);
        
        var mappedWebinars = webinars.Adapt<IEnumerable<WebinarShortInfoDto>>();

        var pages = await GetNumberOfPages(request.ItemsPerPage, cancellationToken);

        var result = new WebinarPagedDto(pages, mappedWebinars);
        
        return Results.Ok(result);
    }

    private async Task<int> GetNumberOfPages(int itemsPerPage, CancellationToken cancellationToken = default)
    {
        var count = await UnitOfWork.WebinarRepository.CountAsync(cancellationToken);
        
        return (int)Math.Ceiling((double)count / itemsPerPage);
    }
}