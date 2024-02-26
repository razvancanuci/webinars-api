using Application.Requests.Interfaces;
using Domain.Extensions;

namespace Application.Requests;

public sealed record CancelWebinarRequest(string WebinarId) : ICommandRequest
{
    public string KeyToDelete { get; set; } = WebinarId.ToWebinarByIdCacheKey();
}
