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
        await _repository.RegisterPersonToWebinarAsync(request.Person, request.WebinarId);
        return new NoContentResult();
    }
}
