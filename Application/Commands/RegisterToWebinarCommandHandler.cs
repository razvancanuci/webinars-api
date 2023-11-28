using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class RegisterToWebinarCommandHandler : IRequestHandler<RegisterWebinarRequest, IActionResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public RegisterToWebinarCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IActionResult> Handle(RegisterWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinarList = await _unitOfWork.WebinarRepository
            .GetAsync(entity => entity.Id == request.WebinarId);
        
        var webinar = webinarList.FirstOrDefault();
        
        if (webinar is null)
        {
            return new NotFoundResult();
        }
        
        webinar.PeopleRegistered.Add(request.Person);
        await _unitOfWork.SaveAsync();
        return new NoContentResult();
    }
}
