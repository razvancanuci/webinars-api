using Application.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Application.Services.Implementations;

public class CacheService : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(10);
    private readonly IDatabase _cache;

    public CacheService(IConnectionMultiplexer multiplexer)
    {
        _cache = multiplexer.GetDatabase();
    }

    public async Task DeleteKeyAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }
    
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> dbQuery, TimeSpan? expiration = null)
    {
        var json = await _cache.StringGetAsync(key);
        
        if (json.IsNullOrEmpty)
        {
            return await ExecuteQuery(key, dbQuery, expiration ?? DefaultExpiration);
        }
        
        var result = JsonConvert.DeserializeObject<T>(json.ToString());
        return result;
    }

    private async Task<T> ExecuteQuery<T>(string key, Func<Task<T>> dbQuery, TimeSpan expiration)
    {
        var queryResult = await dbQuery();
            
        var cache = JsonConvert.SerializeObject(queryResult);
        await _cache.StringSetAsync(key, cache, expiration);
            
        return queryResult;
    }
}