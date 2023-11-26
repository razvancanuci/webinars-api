using Domain.Entities;
using Domain.Requests;
using MediatR;

namespace Application.Queries;

public class GetAvailableWebinarByIdQueryHandler : IRequestHandler<AvailableWebinarByIdRequest, Webinar>
{
    public Task<Webinar> Handle(AvailableWebinarByIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}