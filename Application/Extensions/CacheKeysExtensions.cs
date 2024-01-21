namespace Application.Extensions;

public static class CacheKeysExtensions
{
    public static string ToWebinarByIdCacheKey(this string id) => $"webinar-by-id-{id}";
}