using System.Net;
using ObjectsLoader.Services;

namespace ObjectsLoader.Clients;

public class TranslatorClient
{
    private const string Target = "ru";
    private const string Url = "https://localhost:7001/translate?text={0}&target={1}&source={2}";
    private readonly HttpClient client = new();
    private readonly DistributedCache cache = new();

    public async Task<string?> Fetch(string text, string? source = null)
    {
        var key = $"{Target}|{text}";
        var translation = await cache.GetAsync(key);
        if (translation is not null)
        {
            return translation;
        }
        var query = string.Format(Url, text, Target, source);
        var response = await client.GetAsync(query);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        var stringResponse = await response.Content.ReadAsStringAsync();
        await cache.SetAsync(key, stringResponse);
        return stringResponse;
    }
}