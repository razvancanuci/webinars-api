using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Commands;

public class AddNewWebinarCommandHandler : RequestHandlerBase, ICommandHandler<NewWebinarRequest>
{
    private readonly IFileStorage _fileStorage;
    private readonly IContentModerationService _contentModerationService;
    private readonly ILogger<AddNewWebinarCommandHandler> _logger;

    public AddNewWebinarCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorage fileStorage,
        IContentModerationService contentModerationService,
        ILogger<AddNewWebinarCommandHandler> logger) : base(unitOfWork)
    {
        _fileStorage = fileStorage;
        _contentModerationService = contentModerationService;
        _logger = logger;
    }

    public async Task<IResult> Handle(NewWebinarRequest request, CancellationToken cancellationToken)
    {
        var stream = request.Image.OpenReadStream();

        var isRacyOrAdult = await _contentModerationService.IsRacyOrAdultImage(stream, cancellationToken);

        if (isRacyOrAdult)
        {
            _logger.LogInformation("The client introduced a racy or adult image");
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
        await UnitOfWork.SaveAsync(cancellationToken);
        
        await _fileStorage.CreateAsync(webinar.Id, request.Image, cancellationToken);

        return Results.Created("AddWebinar", request);
    }
}