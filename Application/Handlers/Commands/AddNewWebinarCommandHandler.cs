using Application.Requests;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class AddNewWebinarCommandHandler : RequestHandlerBase, IRequestHandler<NewWebinarRequest, IActionResult>
{
    private readonly IFileStorage _fileStorage;

    public AddNewWebinarCommandHandler(IUnitOfWork unitOfWork, IFileStorage fileStorage) : base(unitOfWork)
    {
        _fileStorage = fileStorage;
    }
    
    public async Task<IActionResult> Handle(NewWebinarRequest request, CancellationToken cancellationToken)
    {
        var webinar = new Webinar
        {
            Title = request.Title,
            Description = request.Description,
            Host = request.Host,
            ScheduleDate = request.DateScheduled
        };
        
        await UnitOfWork.WebinarRepository.InsertAsync(webinar);
        await UnitOfWork.SaveAsync();
        
        if (request.Image is not null)
        {
            var extension = WebinarConstants.AcceptedImageExtensions.First(x => request.Image.FileName.EndsWith(x));
            
            await _fileStorage.CreateAsync($"{webinar.Id}{extension}", request.Image);
        }
        
        return new CreatedResult("AddWebinar", request);
    }
}