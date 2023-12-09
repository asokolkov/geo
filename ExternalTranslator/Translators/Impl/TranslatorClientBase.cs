using ExternalTranslator.Enums;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Models;
using ExternalTranslator.Services;

namespace ExternalTranslator.Translators.Impl;

public class TranslatorClientBase
{
    protected readonly HttpClient HttpClient = new();
    private readonly IDistributedCache cache;
    private readonly ILogger<ITranslatorClient> logger;

    protected TranslatorClientBase(IDistributedCache cache, ILogger<ITranslatorClient> logger)
    {
        this.cache = cache;
        this.logger = logger;
    }

    protected Translator Model { get; init; } = null!;

    public bool CanTranslate(string text)
    {
        logger.LogInformation("Checking if translator can translate");
        foreach (var restriction in Model.Restrictions)
        {
            var charsLimitReached = restriction.Type == RestrictionType.Chars && restriction.CurrentAmount + text.Length >= restriction.MaxAmount;
            var queriesLimitReached = restriction.Type == RestrictionType.Queries && restriction.CurrentAmount + 1 >= restriction.MaxAmount;
            if (charsLimitReached || queriesLimitReached)
            {
                logger.LogInformation("Translator: {Translator} can't translate because of restriction: {Restriction}", Model.Id, restriction.Type.ToString());
                return false;
            }
        }
        logger.LogInformation("Translator: {Translator} can translate", Model.Id);
        return true;
    }
    
    public async Task TryResetRestrictions()
    {
        logger.LogInformation("Trying to reset restrictions");
        foreach (var restriction in Model.Restrictions)
        {
            if (DateTimeOffset.Now - restriction.TimeCheckpoint > restriction.Period)
            {
                restriction.CurrentAmount = 0;
                restriction.TimeCheckpoint = null;
                logger.LogInformation("Translator: {Translator} restriction: {Restriction} reset", Model.Id, restriction.Type.ToString());
            }
        }
        await SaveCache();
    }
    
    protected void UpdateModel(string text)
    {
        foreach (var restriction in Model.Restrictions)
        {
            if (restriction.Type == RestrictionType.Chars)
            {
                restriction.CurrentAmount += text.Length;
                restriction.TimeCheckpoint ??= DateTimeOffset.Now;
            }
            else if (restriction.Type == RestrictionType.Queries)
            {
                restriction.CurrentAmount += 1;
                restriction.TimeCheckpoint ??= DateTimeOffset.Now;
            }
        }
        logger.LogInformation("Translator: {Translator} updated restrictions after text: {Text}", Model.Id, text);
    }
    
    protected void ReadCache()
    {
        var modelJson = cache.Get(Model.Id);
        foreach (var restriction in modelJson.Restrictions)
        {
            var modelRestriction = Model.Restrictions.First(x => x.Type == restriction.Type);
            modelRestriction.CurrentAmount = restriction.CurrentAmount;
            modelRestriction.TimeCheckpoint = restriction.TimeCheckpoint;
        }
        logger.LogInformation("Translator with id: {Id} restrictions read from cache", Model.Id);
    }
    
    protected async Task SaveCache()
    {
        var restrictions = Model.Restrictions
            .Select(restriction => new RestrictionJson
            {
                Type = restriction.Type, 
                CurrentAmount = restriction.CurrentAmount,
                TimeCheckpoint = restriction.TimeCheckpoint
            })
            .ToList();
        var modelJson = new TranslatorJson
        {
            Id = Model.Id,
            Restrictions = restrictions
        };
        await cache.SetAsync(modelJson);
    }
}