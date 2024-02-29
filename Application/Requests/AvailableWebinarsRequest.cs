using Application.Requests.Interfaces;
using Domain.Interfaces.Requests;

namespace Application.Requests;

public class AvailableWebinarsRequest : IQueryRequest, IPaginatedRequest
{
    public int ItemsPerPage { get; } = 2;
    public int Page { get; init; }
}