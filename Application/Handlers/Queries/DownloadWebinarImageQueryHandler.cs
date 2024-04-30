using Application.Handlers.Interfaces;
using Application.Requests;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Queries;

public class DownloadWebinarImageQueryHandler : RequestHandlerBase, IQueryHandler<DownloadWebinarImageRequest>
{
    private readonly IFileStorage _fileStorage;
    public DownloadWebinarImageQueryHandler(IUnitOfWork unitOfWork, IFileStorage fileStorage) 
        : base(unitOfWork)
    {
        _fileStorage = fileStorage;
    }

    public async Task<IResult> Handle(DownloadWebinarImageRequest request, CancellationToken cancellationToken)
    {
        var image = await _fileStorage.GetAsync(request.WebinarId, cancellationToken);

        if (image == default((Stream, string)))
        {
            return Results.NotFound("The image was not found");
        }

        return Results.File(image.Stream, image.ContentType);
    }
}