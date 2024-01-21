namespace Application.Services.Interfaces;

public interface ICacheService
{
     Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> dbQuery, TimeSpan? expiration = null);
     Task DeleteKeyAsync(string key);
}