using Domain.Interfaces;
using Microsoft.Azure.CognitiveServices.ContentModerator;

namespace AIServices;

public class ContentModerationService : IContentModerationService
{
    private readonly IContentModeratorClient _client;

    public ContentModerationService(ContentModeratorClient client)
    {
        _client = client;
    }
    
    public async Task<bool> IsRacyOrAdultImage(Stream stream, CancellationToken cancellationToken = default)
    {
        var result = await _client.ImageModeration.EvaluateFileInputAsync(stream, cancellationToken: cancellationToken);

        var racy = result.IsImageRacyClassified.GetValueOrDefault();
        var adult = result.IsImageAdultClassified.GetValueOrDefault();

        return racy || adult;
    }
}