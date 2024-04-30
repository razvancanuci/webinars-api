using Application.Requests.Interfaces;

namespace Application.Requests;

public sealed record DownloadWebinarImageRequest(string WebinarId) : IQueryRequest
{
}