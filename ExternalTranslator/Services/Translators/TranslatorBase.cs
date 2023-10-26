using System.Net;
using System.Text.Json;
using ExternalTranslator.Enums;
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
        return Model.Restrictions.Aggregate(true, (result, restriction) => restriction.Type switch
        {
            RestrictionType.Chars => result && !restriction.LimitReached(text.Length),
            RestrictionType.Queries => result && !restriction.LimitReached(1),
            RestrictionType.Bytes => result && !restriction.LimitReached(text.Length * sizeof(char)),
            _ => result
        });
    }
    
    public void TryUpdatePeriods()
    {
        var restriction = Model.Restrictions.FirstOrDefault(x => DateTimeOffset.Now - Model.TimeCheckpoint > x.Period);
        if (restriction is not null)
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