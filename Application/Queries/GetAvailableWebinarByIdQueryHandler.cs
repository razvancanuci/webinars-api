using Application.Requests;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Queries;

public class GetAvailableWebinarByIdQueryHandler : IRequestHandler<AvailableWebinarByIdRequest, IActionResult>
{
    private readonly IRepository _repository;
    public GetAvailableWebinarByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<IActionResult> Handle(AvailableWebinarByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetWebinarByIdAsync(request.WebinarId);
        
        if (result is null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(result);
    }
}