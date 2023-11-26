using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;

public class AvailableWebinarByIdRequest : IRequest<IActionResult>
{
    public int WebinarId { get; init; }
}