using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ExternalTranslator.Enums;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Models;
using ExternalTranslator.Options;
using Microsoft.Extensions.Options;

namespace ExternalTranslator.Services.Translators;

internal class YandexClient : IYandexClient
{
    private readonly YandexClientOptions options;
    private readonly IDistributedCache cache;
    private readonly HttpClient httpClient;

    private readonly Translator model = new()
    {
        Id = "Yandex",
        Restrictions = new List<Restriction>
        {
            new()
            {
                Type = RestrictionType.Chars,
                MaxAmount = 1000000,
                Period = TimeSpan.FromHours(1)
            },
            new()
            {
                Type = RestrictionType.Queries,
                MaxAmount = 20,
                Period = TimeSpan.FromSeconds(1)
            }
        }
    };
    
    public YandexClient(IOptions<YandexClientOptions> options, IDistributedCache cache)
    {
        this.cache = cache;
        this.options = options.Value;
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.options.ApiKey);
        ReadCache();
    }

    public async Task TryResetModel()
    {
        var timePassed = model.Restrictions.All(x => DateTimeOffset.Now - model.TimeCheckpoint > x.Period);
        if (!timePassed)
        {
            return;
        }
        foreach (var restriction in model.Restrictions)
        {
            restriction.CurrentAmount = 0;
        }
        model.TimeCheckpoint = null;
        await SaveCache();
    }

    public bool CanTranslate(string text)
    {
        foreach (var restriction in model.Restrictions)
        {
            if (restriction.Type == RestrictionType.Chars && restriction.LimitReached(text.Length) ||
                restriction.Type == RestrictionType.Queries && restriction.LimitReached(1))
            {
                return false;
            }
        }
        return true;
    }

    public async Task<string?> Translate(string text, string? source, string target)
    {
        if (source == target)
        {
            return null;
        }

        var data = new YandexRequestJson
        {
            Target = target,
            Source = source,
            Text = text,
            FolderId = options.ApiFolderId
        };
        var response = await Fetch(data);
        var translation = response?.Translations.FirstOrDefault()?["text"];
        if (translation is null)
        {
            return null;
        }
        UpdateModel(text);
        await SaveCache();
        return translation;
    }

    private void ReadCache()
    {
        var modelJson = cache.Get(model.Id);
        model.TimeCheckpoint = modelJson.TimeCheckpoint;
        foreach (var restriction in modelJson.Restrictions)
        {
            model.Restrictions.First(x => x.Type == restriction.Type).CurrentAmount = restriction.CurrentAmount;
        }
    }
    
    private void UpdateModel(string text)
    {
        model.TimeCheckpoint ??= DateTimeOffset.Now;
        foreach (var restriction in model.Restrictions)
        {
            if (restriction.Type == RestrictionType.Chars)
            {
                restriction.CurrentAmount += text.Length;
            }
            else if (restriction.Type == RestrictionType.Queries)
            {
                restriction.CurrentAmount += 1;
            }
        }
    }
    
    private async Task SaveCache()
    {
        var restrictions = model.Restrictions
            .Select(restriction => new RestrictionJson { Type = restriction.Type, CurrentAmount = restriction.CurrentAmount })
            .ToList();
        var modelJson = new TranslatorJson
        {
            Id = model.Id,
            TimeCheckpoint = model.TimeCheckpoint,
            Restrictions = restrictions
        };
        await cache.SetAsync(modelJson);
    }

    private async Task<YandexResponseJson?> Fetch(YandexRequestJson json)
    {
        var t = JsonSerializer.Serialize(json);
        var data = new StringContent(JsonSerializer.Serialize(json));
        var response = await httpClient.PostAsync(options.ApiUrl, data);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<YandexResponseJson>(content);
    }
}