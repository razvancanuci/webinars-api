using Domain.Constants;
using Domain.Interfaces.Requests;

namespace Domain.Specifications.Webinar;

public class GetWebinarsPaginatedSpecification : Specification<Entities.Webinar>
{
    public GetWebinarsPaginatedSpecification(IPaginatedRequest request)
    : base(criteria: entity => entity.ScheduledAt > WebinarConstants.AvailabilityDate,
        orderByAscending: webinar => webinar.ScheduledAt,
        select: w => new Entities.Webinar {Id = w.Id,
            Description = w.Description,
            Host = w.Host,
            Title = w.Title,
            ScheduledAt = w.ScheduledAt},
        pagination: request,
        asNoTracking: true) { }  
}