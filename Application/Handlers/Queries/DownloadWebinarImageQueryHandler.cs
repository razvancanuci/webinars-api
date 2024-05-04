using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queries;

public class DownloadWebinarImageQueryHandler : RequestHandlerBase, IQueryHandler<DownloadWebinarImageRequest>
{
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<DownloadWebinarImageQueryHandler> _logger;
    public DownloadWebinarImageQueryHandler(
        IUnitOfWork unitOfWork,
        IFileStorage fileStorage,
        ILogger<DownloadWebinarImageQueryHandler> logger) 
        : base(unitOfWork)
    {
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<IResult> Handle(DownloadWebinarImageRequest request, CancellationToken cancellationToken)
    {
        var image = await _fileStorage.GetAsync(request.WebinarId, cancellationToken);

        if (image == default((Stream, string)))
        {
            _logger.LogInformation("The image was not found");
            return Results.NotFound("The image was not found");
        }

        return Results.File(image.Stream, image.ContentType);
    }
}