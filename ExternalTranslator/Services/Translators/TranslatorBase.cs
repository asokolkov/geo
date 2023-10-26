using System.Net;
using System.Text.Json;
using ExternalTranslator.Models;

namespace ExternalTranslator.Services.Translators;

internal abstract class TranslatorBase
{
    private readonly HttpClient client;
    protected readonly CacheService CacheService;
    protected Translator Model { get; init; } = null!;

    protected TranslatorBase(HttpClient client, CacheService cacheService)
    {
        this.client = client;
        CacheService = cacheService;
    }

    public bool CanTranslate(string text)
    {
        var cache = CacheService.GetOrCreate(Model.Id);
        return cache.CurrentCharsAmount + text.Length < Model.CharsMax;
    }
    
    public void TryUpdatePeriods()
    {
        var cache = CacheService.GetOrCreate(Model.Id);
        if (DateTimeOffset.Now - cache.TimeCheckpoint > Model.CharsPeriod)
        {
            CacheService.Reset(Model.Id);
        }
    }

    public abstract Task<string?> Translate(string text, string? source, string target);

    protected async Task<T?> SendQuery<T>(string query)
    {
        var response = await client.GetAsync(query);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content);
    }
}