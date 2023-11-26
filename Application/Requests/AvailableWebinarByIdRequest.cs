using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;
#nullable disable
public class AvailableWebinarByIdRequest : IRequest<IActionResult>
{
    public string WebinarId { get; init; }
}