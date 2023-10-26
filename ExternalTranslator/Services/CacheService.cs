using System.Text.Json;
using ExternalTranslator.Models;

namespace ExternalTranslator.Services;

public class CacheService
{
    private const string CachePath = "./Properties/TranslationCache.json";
    private readonly List<TranslatorCache> translators;
    
    public CacheService()
    {
        if (File.Exists(CachePath))
        {
            using var stream = File.OpenRead(CachePath);
            translators = JsonSerializer.Deserialize<List<TranslatorCache>>(stream)!;
        }
        else
        {
            translators = new List<TranslatorCache>();
        }
    }

    public TranslatorCache GetOrCreate(string translatorId)
    {
        var translator = translators.FirstOrDefault(x => x.Id == translatorId);
        if (translator is not null)
        {
            return translator;
        }

        var newTranslator = new TranslatorCache
        {
            Id = translatorId,
            CurrentCharsAmount = 0,
            TimeCheckpoint = null
        };
        translators.Add(newTranslator);
        return newTranslator;
    }

    public void Update(string translatorId, int charsAmount)
    {
        var translator = translators.First(x => x.Id == translatorId);
        translator.CurrentCharsAmount += charsAmount;
        translator.TimeCheckpoint ??= DateTimeOffset.Now;
        Save();
    }
    
    public void Reset(string translatorId)
    {
        var translator = translators.First(x => x.Id == translatorId);
        translator.CurrentCharsAmount = 0;
        translator.TimeCheckpoint = null;
        Save();
    }
    
    private void Save()
    {
        File.WriteAllText(CachePath, JsonSerializer.Serialize(translators));
    }
}