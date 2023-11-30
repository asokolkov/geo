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
    private readonly ILogger<YandexClient> logger;
    
    public YandexClient(IOptions<YandexClientOptions> options, IDistributedCache cache, ILogger<YandexClient> logger) : base(cache, logger)
    {
        this.logger = logger;
        this.options = options.Value;
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Api-Key", this.options.ApiKey);
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
        logger.LogInformation("{{msg=\"YandexClient initialized\"}}");
    }

    public async Task<string?> Translate(string text, string? source, string target)
    {
        if (source == target)
        {
            logger.LogInformation("{{msg=\"Source language is equal to target language, returning null\"}}");
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
            logger.LogInformation("{{msg=\"Translation failed, returning null\"}}");
            return null;
        }
        UpdateModel(text);
        await SaveCache();
        
        var logMessageText = translation.Length <= 10 ? translation : translation[..10] + "...";
        logger.LogInformation("{{msg=\"Successful translation {Translation}\"}}", logMessageText);
        return translation;
    }

    private async Task<YandexResponseJson?> Fetch(YandexRequestJson json)
    {
        var data = new StringContent(JsonSerializer.Serialize(json));
        var response = await HttpClient.PostAsync(options.ApiUrl, data);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("{{msg=\"Response code is {Code}, returning default value\"}}", response.StatusCode);
            return default;
        }
        logger.LogInformation("{{msg=\"Successful request to Yandex translator API\"}}");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<YandexResponseJson>(content);
    }
}