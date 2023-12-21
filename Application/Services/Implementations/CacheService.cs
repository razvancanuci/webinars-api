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

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> dbQuery, TimeSpan? expiration = null)
    {
        var json = await _cache.StringGetAsync(key);
        
        if (json.IsNullOrEmpty)
        {
            var queryResult = await dbQuery();
            
            var cache = JsonConvert.SerializeObject(queryResult);
            await _cache.StringSetAsync(key, cache, expiration ?? DefaultExpiration);
            
            return queryResult;
        }
        
        var result = JsonConvert.DeserializeObject<T>(json.ToString());
        return result;
    }
}