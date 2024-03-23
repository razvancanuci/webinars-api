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
        var webinars = await UnitOfWork.WebinarRepository
            .GetAsync(new GetWebinarsPaginatedSpecification(request));
        
        var mappedWebinars = webinars.Adapt<IEnumerable<WebinarShortInfoDto>>();

        var pages = await GetNumberOfPages(request.ItemsPerPage);

        var result = new WebinarPagedDto(pages, mappedWebinars);
        
        return Results.Ok(result);
    }

    private async Task<int> GetNumberOfPages(int itemsPerPage)
    {
        var count = await UnitOfWork.WebinarRepository.CountAsync();
        
        return (int)Math.Ceiling((double)count / itemsPerPage);
    }
}