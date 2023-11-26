using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class RegisterToWebinarCommandHandler : IRequestHandler<RegisterWebinarRequest, IActionResult>
{
    private readonly IRepository _repository;
    public RegisterToWebinarCommandHandler(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IActionResult> Handle(RegisterWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinar = await _repository.GetWebinarByIdAsync(request.WebinarId);
        
        if (webinar is null)
        {
            return new NotFoundResult();
        }
        
        webinar.PeopleRegistered.Add(request.Person);
        await _repository.RegisterPersonToWebinarAsync(webinar);
        return new NoContentResult();
    }
}
