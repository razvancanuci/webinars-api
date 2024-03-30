using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Commands;

public class AddNewWebinarCommandHandler : RequestHandlerBase, ICommandHandler<NewWebinarRequest>
{
    private readonly IFileStorage _fileStorage;
    private readonly IContentModerationService _contentModerationService;

    public AddNewWebinarCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorage fileStorage,
        IContentModerationService contentModerationService) : base(unitOfWork)
    {
        _fileStorage = fileStorage;
        _contentModerationService = contentModerationService;
    }

    public async Task<IResult> Handle(NewWebinarRequest request, CancellationToken cancellationToken)
    {
        var stream = request.Image.OpenReadStream();

        var isRacyOrAdult = await _contentModerationService.IsRacyOrAdultImage(stream);

        if (isRacyOrAdult)
        {
            return Results.BadRequest("The image contains adult or racy content");
        }
        
        var webinar = new Webinar
        {
            Title = request.Title,
            Description = request.Description,
            Host = request.Host,
            ScheduleDate = request.DateScheduled
        };
        
        cancellationToken.ThrowIfCancellationRequested();

        await UnitOfWork.WebinarRepository.InsertAsync(webinar);
        await UnitOfWork.SaveAsync();
        
        var extension = WebinarConstants.AcceptedImageExtensions.First(x => request.Image.FileName.EndsWith(x));

        await _fileStorage.CreateAsync($"{webinar.Id}{extension}", request.Image);

        return Results.Created("AddWebinar", request);
    }
}