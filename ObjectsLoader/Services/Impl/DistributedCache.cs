using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Services.Impl;

public class DistributedCache : IDistributedCache
{
    private readonly ILogger<DistributedCache> logger;
    private readonly MemoryCache cache;

    public DistributedCache(ILogger<DistributedCache> logger)
    {
        this.logger = logger;
        cache = new MemoryCache(new MemoryCacheOptions());
        logger.LogInformation("DistributedCache initialized");
    }
    
    public string? Get(string key)
    {
        logger.LogInformation("Trying to find value by key: {Key} in cache", key);
        cache.TryGetValue<string>(key, out var result);
        if (result is not null)
        {
            logger.LogInformation("Found value: {Value} for key: {Key}", result, key);
        }
        else
        {
            logger.LogInformation("Key: {Key} not found, returning null", key);
        }
        return result;
    }

    public async Task<string?> GetAsync(string key)
    {
        logger.LogInformation("Async getting from cache method called, creating task with default Get method");
        return await Task.FromResult(Get(key));
    }

    public void Set(string key, string value)
    {
        logger.LogInformation("Trying to set value: {Value} by key: {Key} in cache", value, key);
        cache.Set(key, value);
        logger.LogInformation("Set value: {Value} for key: {Key} in cache", value, key);
    }

    public async Task SetAsync(string key, string value)
    {
        logger.LogInformation("Async setting to cache method called, creating task with default Set method");
        Set(key, value);
        await Task.CompletedTask;
    }
}