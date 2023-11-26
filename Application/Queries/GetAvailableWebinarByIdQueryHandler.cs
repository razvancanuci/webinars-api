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
        await _repository.GetWebinarByIdAsync(request.WebinarId);
        return new OkObjectResult(new Webinar { Id = request.WebinarId });
    }
}