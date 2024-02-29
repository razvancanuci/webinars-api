﻿using Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Services.Implementations;

public class CacheService : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(10);
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task DeleteKeyAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
    
    public async Task<T> GetOrCreateAsync<T>(string key, Func<ValueTask<T>> dbQuery, TimeSpan? expiration = null)
    {
        var json = await _cache.GetStringAsync(key);
        
        if (string.IsNullOrEmpty(json))
        {
            return await ExecuteQuery(key, dbQuery, expiration ?? DefaultExpiration);
        }
        
        var result = JsonSerializer.Deserialize<T>(json);
        return result!;
    }

    private async Task<T> ExecuteQuery<T>(string key, Func<ValueTask<T>> dbQuery, TimeSpan expiration)
    {
        var queryResult = await dbQuery();
            
        var cache = JsonSerializer.Serialize(queryResult);
        
        await _cache.SetStringAsync(
            key, 
            cache, 
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
            
        return queryResult;
    }
}