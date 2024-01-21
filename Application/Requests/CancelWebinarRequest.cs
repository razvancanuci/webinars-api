using Application.Extensions;
using Application.Requests.Interfaces;

namespace Application.Requests;

public sealed record CancelWebinarRequest(string WebinarId) : ICommandRequest
{
    public string KeyToDelete { get; set; } = WebinarId.ToWebinarByIdCacheKey();
}
