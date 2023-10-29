using ExternalTranslator.Enums;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Models;

namespace ExternalTranslator.Services.Translators;

public class TranslatorClientBase
{
    protected readonly HttpClient HttpClient = new();
    private readonly IDistributedCache cache;

    protected TranslatorClientBase(IDistributedCache cache)
    {
        this.cache = cache;
    }

    protected Translator Model { get; init; } = null!;

    public bool CanTranslate(string text)
    {
        foreach (var restriction in Model.Restrictions)
        {
            if (restriction.Type == RestrictionType.Chars && restriction.LimitReached(text.Length) ||
                restriction.Type == RestrictionType.Queries && restriction.LimitReached(1))
            {
                return false;
            }
        }
        return true;
    }
    
    public async Task TryResetModel()
    {
        // TODO: это неправильно, нужно обновлять каждое ограничение отдельно
        var timePassed = Model.Restrictions.All(x => DateTimeOffset.Now - Model.TimeCheckpoint > x.Period);
        if (!timePassed)
        {
            return;
        }
        foreach (var restriction in Model.Restrictions)
        {
            restriction.CurrentAmount = 0;
        }
        Model.TimeCheckpoint = null;
        await SaveCache();
    }
    
    protected void UpdateModel(string text)
    {
        Model.TimeCheckpoint ??= DateTimeOffset.Now;
        foreach (var restriction in Model.Restrictions)
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
    
    protected void ReadCache()
    {
        var modelJson = cache.Get(Model.Id);
        Model.TimeCheckpoint = modelJson.TimeCheckpoint;
        foreach (var restriction in modelJson.Restrictions)
        {
            Model.Restrictions.First(x => x.Type == restriction.Type).CurrentAmount = restriction.CurrentAmount;
        }
    }
    
    protected async Task SaveCache()
    {
        var restrictions = Model.Restrictions
            .Select(restriction => new RestrictionJson { Type = restriction.Type, CurrentAmount = restriction.CurrentAmount })
            .ToList();
        var modelJson = new TranslatorJson
        {
            Id = Model.Id,
            TimeCheckpoint = Model.TimeCheckpoint,
            Restrictions = restrictions
        };
        await cache.SetAsync(modelJson);
    }
}