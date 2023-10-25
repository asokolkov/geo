using System.Net;
using System.Text.Json;
using ExternalTranslator.Models;

namespace ExternalTranslator.Services.Translators;

internal abstract class TranslatorBase
{
    private readonly HttpClient client;
    protected readonly CacheService CacheService;
    protected TranslatorCache Cache { get; init; } = null!;
    protected Translator Model { get; init; } = null!;

    protected TranslatorBase(HttpClient client, CacheService cacheService)
    {
        this.client = client;
        CacheService = cacheService;
    }

    public bool CanTranslate(string text)
    {
        var charsAmountValid = Cache.CurrentCharsAmount + text.Length < Model.CharsMax;
        var queriesAmountValid = Cache.CurrentQueriesAmount + 1 < Model.QueriesMax;
        return charsAmountValid && queriesAmountValid;
    }
    
    public void TryUpdatePeriods()
    {
        var updated = false;
        if (DateTimeOffset.Now - Cache.TimeCheckpoint > Model.CharsPeriod)
        {
            Cache.CurrentCharsAmount = 0;
            updated = true;
        }
        if (DateTimeOffset.Now - Cache.TimeCheckpoint > Model.QueriesPeriod)
        {
            Cache.CurrentQueriesAmount = 0;
            updated = true;
        }
        if (updated)
        {
            Cache.TimeCheckpoint = null;
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

    protected void UpdateCache(string text)
    {
        Cache.CurrentCharsAmount += text.Length;
        Cache.CurrentQueriesAmount += 1;
        Cache.TimeCheckpoint ??= DateTimeOffset.Now;
    }
}