using System.Text.Json;
using ExternalTranslator.JsonModels;
using Microsoft.Extensions.Caching.Memory;

namespace ExternalTranslator.Services;

public class InMemoryDistributedCache : IDistributedCache
{
    private readonly IMemoryCache cache;

    public InMemoryDistributedCache(IMemoryCache cache)
    {
        this.cache = cache;
    }
    
    public TranslatorJson Get(string key)
    {
        var cacheString = cache.Get<string>(key);
        if (cacheString is null)
        {
            return new TranslatorJson();
        }
        var json = JsonSerializer.Deserialize<TranslatorJson>(cacheString);
        return json ?? new TranslatorJson();
    }

    public async Task<TranslatorJson> GetAsync(string key)
    {
        return await Task.FromResult(Get(key));
    }

    public void Set(TranslatorJson translatorJson)
    {
        var json = JsonSerializer.Serialize(translatorJson);
        cache.Set(translatorJson.Id, json);
    }

    public async Task SetAsync(TranslatorJson translatorJson)
    {
        Set(translatorJson);
        await Task.CompletedTask;
    }
}