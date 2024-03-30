namespace Domain.Interfaces;

public interface IContentModerationService
{
    Task<bool> IsRacyOrAdultImage(Stream stream, CancellationToken cancellationToken = default);
}