using System.Net;
using System.Text.Json;
using DetectLanguage;
using ExternalTranslator.Enums;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Models;
using ExternalTranslator.Options;
using ExternalTranslator.Services;
using Microsoft.Extensions.Options;

namespace ExternalTranslator.Translators.Impl;

internal class MyMemoryClient : TranslatorClientBase, ITranslatorClient
{
    private readonly MyMemoryClientOptions options;
    private readonly DetectLanguageClient languageIdentifier;
    
    public MyMemoryClient(IOptions<MyMemoryClientOptions> options, IDistributedCache cache) : base(cache)
    {
        this.options = options.Value;
        languageIdentifier = new DetectLanguageClient(this.options.DetectLanguageApiKey);
        Model = new Translator
        {
            Id = "MyMemory",
            Restrictions = new List<Restriction>
            {
                new()
                {
                    Type = RestrictionType.Chars,
                    MaxAmount = 5000,
                    Period = TimeSpan.FromDays(1)
                }
            }
        };
        ReadCache();
    }

    public async Task<string?> Translate(string text, string? source, string target)
    {
        var validSource = source ?? await IdentifySource(text);
        if (source == target)
        {
            return null;
        }
        var url = string.Format(options.ApiUrl, text, validSource, target);
        var response = await Fetch(url);
        var translation = response?.Response?.TranslatedText;
        if (translation is null)
        {
            return null;
        }
        UpdateModel(text);
        await SaveCache();
        return translation;
    }

    private async Task<MyMemoryJson?> Fetch(string url)
    {
        var response = await HttpClient.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MyMemoryJson>(content);
    }
    
    private async Task<string> IdentifySource(string text)
    {
        var languageResponse = await languageIdentifier.DetectAsync(text);
        return languageResponse.FirstOrDefault(x => x.reliable)?.language ?? "en";
    }
}