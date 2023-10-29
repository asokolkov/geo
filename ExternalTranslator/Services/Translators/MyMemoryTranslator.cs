﻿using System.Net;
using System.Text.Json;
using DetectLanguage;
using ExternalTranslator.Enums;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Models;
using ExternalTranslator.Options;
using Microsoft.Extensions.Options;

namespace ExternalTranslator.Services.Translators;

internal class MyMemoryClient : IMyMemoryClient
{
    private readonly MyMemoryClientOptions options;
    private readonly IDistributedCache cache;
    private readonly HttpClient httpClient;
    private readonly DetectLanguageClient languageIdentifier;

    private readonly Translator model = new()
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
    
    public MyMemoryClient(IOptions<MyMemoryClientOptions> options, IDistributedCache cache)
    {
        this.cache = cache;
        this.options = options.Value;
        httpClient = new HttpClient();
        languageIdentifier = new DetectLanguageClient(this.options.DetectLanguageApiKey);
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
        var validSource = source ?? await IdentifySource(text);
        if (source == target)
        {
            return null;
        }
        var url = string.Format(options.ApiUrl, text, validSource, target);
        var response = await Fetch(url);
        var translation = response?.Response.TranslatedText;
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

    private async Task<MyMemoryJson?> Fetch(string url)
    {
        var response = await httpClient.GetAsync(url);
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