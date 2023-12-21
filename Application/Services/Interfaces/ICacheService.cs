namespace Application.Services.Interfaces;

public interface ICacheService
{
    public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> dbQuery, TimeSpan? expiration = null);
}