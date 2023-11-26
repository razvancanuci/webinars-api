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
        await _repository.GetWebinarsAsync();
        return  new OkObjectResult(new List<Webinar>{new Webinar{Title = "Webinar cu Vasile Lup"}});
    }
}