using System.Net;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Services;

namespace ObjectsLoader.Clients.Impl;

public class TranslatorClient : ClientBase, ITranslatorClient
{
    private const string Url = "https://localhost:7001/translate?text={0}&target={1}&source={2}";
    private readonly ILogger<TranslatorClient> logger;
    private readonly IDistributedCache cache;

    public TranslatorClient(ILogger<TranslatorClient> logger, IDistributedCache cache)
    {
        this.logger = logger;
        this.cache = cache;
    }

    public async Task<string?> Fetch(string text, string target, string? source = null)
    {
        var key = $"{target}|{text}";
        var translation = await cache.GetAsync(key);
        if (translation is not null)
        {
            return translation;
        }
        logger.LogInformation("{{method=\"fetch\" msg=\"Translation not found in cache, making request\"}}");
        var query = string.Format(Url, text, target, source);
        var response = await SendRequest(query);
        if (response is null)
        {
            logger.LogInformation("{{method=\"fetch\" status=\"fail\" msg=\"Bad response\"}}");
            return null;
        }
        logger.LogInformation("{{method=\"fetch\" http_method=\"{Method}\" uri=\"{Uri}\" status_code=\"{Code}\" msg=\"Got response\"}}", response.RequestMessage?.Method, response.RequestMessage?.RequestUri, response.StatusCode);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("{{method=\"fetch\" status=\"fail\" msg=\"Bad response status code\"}}");
            return null;
        }
        var stringResponse = await response.Content.ReadAsStringAsync();
        await cache.SetAsync(key, stringResponse);
        return stringResponse;
    }
}