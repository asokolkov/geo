using System.Net;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Services;

namespace ObjectsLoader.Clients.Impl;

public class TranslatorClient : ClientBase, ITranslatorClient
{
    private const string Url = "https://localhost:7001/translate?text={0}&target={1}&source={2}";
    private readonly IDistributedCache cache;

    public TranslatorClient(ILogger<TranslatorClient> logger, IDistributedCache cache) : base(logger)
    {
        this.cache = cache;
        Logger.LogInformation("TranslatorClient initialized with url: '{Url}'", Url);
    }

    public async Task<string?> Fetch(string text, string target, string? source = null)
    {
        Logger.LogInformation("Translating text: '{Text}' with target language: {Target} and source language: {Source}", text, target, source);
        
        Logger.LogInformation("Trying to find translation in cache");
        var key = $"{target}|{text}";
        var translation = await cache.GetAsync(key);
        if (translation is not null)
        {
            Logger.LogInformation("Returning found in cache translation: {Translation}", translation);
            return translation;
        }
        
        Logger.LogInformation("Translation not found in cache, sending request");
        var query = string.Format(Url, text, target, source);
        var response = await SendRequest(query);
        if (response is null)
        {
            Logger.LogInformation("Failed to send request, returning null");
            return null;
        }
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Logger.LogInformation("Response status code is not 200, returning null");
            return null;
        }
        
        Logger.LogInformation("Response status code is 200, saving response content to cache and returning it");
        var stringResponse = await response.Content.ReadAsStringAsync();
        await cache.SetAsync(key, stringResponse);
        return stringResponse;
    }
}