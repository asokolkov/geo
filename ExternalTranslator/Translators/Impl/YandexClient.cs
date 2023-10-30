using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using ExternalTranslator.Enums;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Models;
using ExternalTranslator.Options;
using ExternalTranslator.Services;
using Microsoft.Extensions.Options;

namespace ExternalTranslator.Translators.Impl;

internal class YandexClient : TranslatorClientBase, ITranslatorClient
{
    private readonly YandexClientOptions options;
    
    public YandexClient(IOptions<YandexClientOptions> options, IDistributedCache cache) : base(cache)
    {
        this.options = options.Value;
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.options.ApiKey);
        Model = new Translator
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
                },
                new() // для тестирования
                {
                    Type = RestrictionType.Queries,
                    MaxAmount = 5000,
                    Period = TimeSpan.FromDays(1)
                }
            }
        };
        ReadCache();
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

    private async Task<YandexResponseJson?> Fetch(YandexRequestJson json)
    {
        var data = new StringContent(JsonSerializer.Serialize(json));
        var response = await HttpClient.PostAsync(options.ApiUrl, data);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<YandexResponseJson>(content);
    }
}