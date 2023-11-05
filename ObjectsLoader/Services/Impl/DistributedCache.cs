using Microsoft.Extensions.Caching.Memory;

namespace ObjectsLoader.Services.Impl;

public class DistributedCache : IDistributedCache
{
    private readonly MemoryCache cache = new(new MemoryCacheOptions());
    
    public string? Get(string key)
    {
        cache.TryGetValue<string>(key, out var result);
        return result;
    }

    public async Task<string?> GetAsync(string key)
    {
        return await Task.FromResult(Get(key));
    }

    public void Set(string key, string value)
    {
        cache.Set(key, value);
    }

    public async Task SetAsync(string key, string value)
    {
        Set(key, value);
        await Task.CompletedTask;
    }
}