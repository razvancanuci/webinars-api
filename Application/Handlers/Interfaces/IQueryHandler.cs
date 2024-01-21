using Application.Requests.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Interfaces;

public interface IQueryHandler<T> : IRequestHandler<T, IActionResult>
    where T: IQueryRequest
{
    
}