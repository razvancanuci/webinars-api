using Application.Requests.Interfaces;

namespace Application.Requests;

public class AvailableWebinarsRequest : IQueryRequest, IPaginatedRequest
{
    public int ItemsPerPage { get; } = 2;
    public int Page { get; init; }
}