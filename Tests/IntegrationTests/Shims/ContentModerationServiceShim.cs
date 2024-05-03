using Domain.Interfaces;

namespace IntegrationTests.Shims;

public class ContentModerationServiceShim : IContentModerationService
{
    public Task<bool> IsRacyOrAdultImage(Stream stream, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}