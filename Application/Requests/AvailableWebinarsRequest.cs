using Application.Requests.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;

public class AvailableWebinarsRequest : IRequest<IActionResult>, IPaginatedRequest
{
    public int ItemsPerPage { get; } = 2;
    public int Page { get; init; }
}