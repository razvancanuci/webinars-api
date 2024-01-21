using Application.Requests.Interfaces;

namespace Application.Requests;
#nullable disable
public record AvailableWebinarByIdRequest(string WebinarId) : IQueryRequest
{
    public string Key => $"webinar-by-id-{WebinarId}";
    public TimeSpan? Expiration => TimeSpan.FromHours(1);
}