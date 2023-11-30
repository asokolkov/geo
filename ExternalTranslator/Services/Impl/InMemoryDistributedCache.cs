using System.Text.Json;
using ExternalTranslator.JsonModels;
using Microsoft.Extensions.Caching.Memory;

namespace ExternalTranslator.Services.Impl;

public class InMemoryDistributedCache : IDistributedCache
{
    private readonly IMemoryCache cache;
    private readonly ILogger<InMemoryDistributedCache> logger;

    public InMemoryDistributedCache(IMemoryCache cache, ILogger<InMemoryDistributedCache> logger)
    {
        this.cache = cache;
        this.logger = logger;
        logger.LogInformation("{{msg=\"InMemoryDistributedCache initialized\"}}");
    }
    
    public TranslatorJson Get(string key)
    {
        var cacheString = cache.Get<string>(key);
        if (cacheString is null)
        {
            logger.LogInformation("{{msg=\"Translation not found in cache\"}}");
            return new TranslatorJson();
        }
        var json = JsonSerializer.Deserialize<TranslatorJson>(cacheString);
        if (json is not null)
        {
            logger.LogInformation("{{msg=\"Translation found in cache with key {Key}\"}}", key);
            return json;
        }

        logger.LogInformation("{{msg=\"Translation found in cache with key {Key} but could not be deserialized\"}}", key);
        return  new TranslatorJson();
    }

    public async Task<TranslatorJson> GetAsync(string key)
    {
        logger.LogInformation("{{msg=\"Async Get called\"}}");
        return await Task.FromResult(Get(key));
    }

    public void Set(TranslatorJson translatorJson)
    {
        var json = JsonSerializer.Serialize(translatorJson);
        cache.Set(translatorJson.Id, json);
        logger.LogInformation("{{msg=\"Translation {Translation} set in cache\"}}", json);
    }

    public async Task SetAsync(TranslatorJson translatorJson)
    {
        logger.LogInformation("{{msg=\"Async Set called\"}}");
        Set(translatorJson);
        await Task.CompletedTask;
    }
}