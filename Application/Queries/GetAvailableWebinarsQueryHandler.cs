using Domain.Entities;
using Domain.Requests;
using MediatR;

namespace Application.Queries;

public class GetAvailableWebinarsQueryHandler : IRequestHandler<AvailableWebinarsRequest, IQueryable<Webinar>>
{
    public GetAvailableWebinarsQueryHandler()
    {
        
    }
    
    public Task<IQueryable<Webinar>> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}