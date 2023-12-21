using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;
#nullable disable
public record AvailableWebinarByIdRequest(string WebinarId) : IRequest<IActionResult>
{
    public string Key => $"webinar-by-id-{WebinarId}";
    public TimeSpan? Expiration => TimeSpan.FromHours(1);
}