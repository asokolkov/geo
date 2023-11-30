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
        foreach (var restriction in Model.Restrictions)
        {
            var charsLimitReached = restriction.Type == RestrictionType.Chars && restriction.CurrentAmount + text.Length >= restriction.MaxAmount;
            var queriesLimitReached = restriction.Type == RestrictionType.Queries && restriction.CurrentAmount + 1 >= restriction.MaxAmount;
            if (charsLimitReached || queriesLimitReached)
            {
                logger.LogInformation("{{msg=\"Translator {Translator} can't translate because of restriction {Restriction}\"}}", Model.Id, restriction.Type.ToString());
                return false;
            }
        }
        logger.LogInformation("{{msg=\"Translator {Translator} can translate\"}}", Model.Id);
        return true;
    }
    
    public async Task TryResetRestrictions()
    {
        foreach (var restriction in Model.Restrictions)
        {
            if (DateTimeOffset.Now - restriction.TimeCheckpoint > restriction.Period)
            {
                restriction.CurrentAmount = 0;
                restriction.TimeCheckpoint = null;
                logger.LogInformation("{{msg=\"Translator {Translator} restriction {Restriction} reset\"}}", Model.Id, restriction.Type.ToString());
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
        var logMessageText = text.Length <= 10 ? text : text[..10] + "...";
        logger.LogInformation("{{msg=\"Translator {Translator} updated restrictions for text {Text}\"}}", Model.Id, logMessageText);
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