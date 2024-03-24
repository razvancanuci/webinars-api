namespace Domain.Interfaces;

public interface IContentModerationService
{
    Task<bool> IsRacyOrAdultImage(Stream stream);
}