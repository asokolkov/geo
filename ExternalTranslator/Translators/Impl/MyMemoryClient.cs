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
    private readonly ILogger<MyMemoryClient> logger;
    
    public MyMemoryClient(IOptions<MyMemoryClientOptions> options, IDistributedCache cache, ILogger<MyMemoryClient> logger) : base(cache, logger)
    {
        this.logger = logger;
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
        logger.LogInformation("{{msg=\"MyMemoryClient initialized\"}}");
    }

    public async Task<string?> Translate(string text, string? source, string target)
    {
        var validSource = source ?? await IdentifySource(text);
        if (source == target)
        {
            logger.LogInformation("{{msg=\"Source language is equal to target language, returning null\"}}");
            return null;
        }
        var url = string.Format(options.ApiUrl, text, validSource, target);
        logger.LogInformation("{{msg=\"API URL built '{Url}'\"}}", url);
        var response = await Fetch(url);
        var translation = response?.Response?.TranslatedText;
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

    private async Task<MyMemoryJson?> Fetch(string url)
    {
        var response = await HttpClient.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("{{msg=\"Response code is {Code}, returning default value\"}}", response.StatusCode);
            return default;
        }
        logger.LogInformation("{{msg=\"Successful request to MyMemory translator API\"}}");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MyMemoryJson>(content);
    }
    
    private async Task<string> IdentifySource(string text)
    {
        var languageResponse = await languageIdentifier.DetectAsync(text);
        var bestLanguage = languageResponse.FirstOrDefault(x => x.reliable)?.language;
        if (bestLanguage is not null)
        {
            var logMessageText = text.Length <= 10 ? text : text[..10] + "...";
            logger.LogInformation("{{msg=\"Source language of text '{Text}' is {Source}\"}}", logMessageText, bestLanguage);
            return bestLanguage;
        }
        logger.LogInformation("{{msg=\"Can't identify source language, returning default EN\"}}");
        return "en";
    }
}