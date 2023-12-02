using Application.Requests;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class RegisterToWebinarCommandHandler : RequestHandlerBase, IRequestHandler<RegisterWebinarRequest, IActionResult>
{
    public RegisterToWebinarCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    
    public async Task<IActionResult> Handle(RegisterWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinarList = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.Id == request.WebinarId);
        
        var webinar = webinarList.FirstOrDefault();
        
        if (webinar is null)
        {
            return new NotFoundResult();
        }
        
        webinar.PeopleRegistered.Add(request.Person);
        await UnitOfWork.SaveAsync();
        return new NoContentResult();
    }
}
