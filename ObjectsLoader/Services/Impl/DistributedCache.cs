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
        logger.LogInformation("{{method=\"distributed_cache_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }
    
    public string? Get(string key)
    {
        cache.TryGetValue<string>(key, out var result);
        if (result is not null)
        {
            logger.LogInformation("{{method=\"get\" status=\"success\" msg=\"Found {Value} for key {Key}\"}}", result, key);
        }
        else
        {
            logger.LogInformation("{{method=\"get\" status=\"fail\" msg=\"Key {Key} not found\"}}", key);
        }
        return result;
    }

    public async Task<string?> GetAsync(string key)
    {
        return await Task.FromResult(Get(key));
    }

    public void Set(string key, string value)
    {
        cache.Set(key, value);
        logger.LogInformation("{{method=\"set\" status=\"success\" msg=\"Set {Value} for key {Key} not found\"}}", value, key);
    }

    public async Task SetAsync(string key, string value)
    {
        Set(key, value);
        await Task.CompletedTask;
    }
}