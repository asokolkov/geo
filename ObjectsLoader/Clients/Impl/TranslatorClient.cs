using System.Net;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Services;

namespace ObjectsLoader.Clients.Impl;

public class TranslatorClient : ITranslatorClient
{
    private readonly ILogger<TranslatorClient> logger;
    private readonly IDistributedCache cache;
    private readonly HttpClient client = new();

    public TranslatorClient(ILogger<TranslatorClient> logger, IDistributedCache cache)
    {
        this.logger = logger;
        this.cache = cache;
        this.logger.LogInformation("TranslatorClient initialized");
    }

    public async Task<string?> Fetch(string text, string target, string? source = null)
    {
        logger.LogInformation("Translating text: '{Text}' with target language: {Target} and source language: {Source}", text, target, source);
        
        logger.LogInformation("Trying to find translation in cache");
        var key = $"{target}|{text}";
        var translation = await cache.GetAsync(key);
        if (translation is not null)
        {
            logger.LogInformation("Returning found in cache translation: {Translation}", translation);
            return translation;
        }
        
        logger.LogInformation("Translation not found in cache, sending request");
        var query = source is null 
            ? $"https://localhost:7001/translate?text={text}&target={target}"
            : $"https://localhost:7001/translate?text={text}&target={target}&source={source}";

        HttpResponseMessage? response;
        try
        {
            response = await client.GetAsync(query);
            logger.LogInformation("Request sent without exceptions, returning response");
        }
        catch (HttpRequestException _)
        {
            logger.LogInformation("Got exception on request with query: '{Query}', returning null", query);
            response = null;
        }
        
        if (response is null)
        {
            logger.LogInformation("Failed to send request, returning null");
            return null;
        }
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("Response status code is not 200, returning null");
            return null;
        }
        
        logger.LogInformation("Response status code is 200, saving response content to cache and returning it");
        var stringResponse = await response.Content.ReadAsStringAsync();
        await cache.SetAsync(key, stringResponse);
        return stringResponse;
    }
}