using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;

public class AvailableWebinarsRequest : IRequest<IActionResult>
{
    public int ItemsPerPage { get; } = 20;
    public int Page { get; init; }
}