using Application.Requests;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Queries;

public class GetAvailableWebinarsQueryHandler : IRequestHandler<AvailableWebinarsRequest, IActionResult>
{
    private readonly IRepository _repository;
    public GetAvailableWebinarsQueryHandler(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IActionResult> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetWebinarsAsync(request.Page, request.ItemsPerPage);
        
        return  new OkObjectResult(result);
    }
}