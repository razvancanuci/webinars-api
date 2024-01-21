using Application.Requests.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Interfaces;

public interface ICommandHandler<T> : IRequestHandler<T, IActionResult>
where T: ICommandRequest
{
    
}