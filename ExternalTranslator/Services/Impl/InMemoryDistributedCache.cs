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
        logger.LogInformation("InMemoryDistributedCache initialized");
    }
    
    public TranslatorJson Get(string key)
    {
        var cacheString = cache.Get<string>(key);
        if (cacheString is null)
        {
            logger.LogInformation("Translator not found in cache, returning empty translator");
            return new TranslatorJson();
        }
        var json = JsonSerializer.Deserialize<TranslatorJson>(cacheString);
        if (json is not null)
        {
            logger.LogInformation("Translator found in cache with key: {Key}", key);
            return json;
        }

        logger.LogInformation("Translation found in cache with key: {Key} but could not be deserialized", key);
        return  new TranslatorJson();
    }

    public async Task<TranslatorJson> GetAsync(string key)
    {
        logger.LogInformation("Async getting from cache method called, creating task with default Get method");
        return await Task.FromResult(Get(key));
    }

    public void Set(TranslatorJson translatorJson)
    {
        var json = JsonSerializer.Serialize(translatorJson);
        cache.Set(translatorJson.Id, json);
        logger.LogInformation("Translator: {Translation} set in cache", json);
    }

    public async Task SetAsync(TranslatorJson translatorJson)
    {
        logger.LogInformation("Async setting to cache method called, creating task with default Set method");
        Set(translatorJson);
        await Task.CompletedTask;
    }
}