using Application.Requests.Interfaces;
using Domain.Extensions;

namespace Application.Requests;
#nullable disable
public sealed record AvailableWebinarByIdRequest(string WebinarId) : IQueryRequest
{
    public string Key => WebinarId.ToWebinarByIdCacheKey();
    public TimeSpan? Expiration => TimeSpan.FromHours(1);
}