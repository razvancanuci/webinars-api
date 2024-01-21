using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests.Interfaces;

public interface ICommandRequest : IRequest<IActionResult>
{
}