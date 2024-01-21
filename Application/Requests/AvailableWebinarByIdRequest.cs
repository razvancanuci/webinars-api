using Application.Extensions;
using Application.Requests.Interfaces;

namespace Application.Requests;
#nullable disable
public sealed record AvailableWebinarByIdRequest(string WebinarId) : IQueryRequest
{
    public string Key => WebinarId.ToWebinarByIdCacheKey();
    public TimeSpan? Expiration => TimeSpan.FromHours(1);
}