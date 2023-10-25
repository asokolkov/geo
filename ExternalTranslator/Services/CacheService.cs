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

    public TranslatorCache Get(string translatorId)
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
            CurrentQueriesAmount = 0,
            TimeCheckpoint = null
        };
        translators.Add(newTranslator);
        return newTranslator;
    }

    public void Save(string translatorId)
    {
        File.WriteAllText(CachePath, JsonSerializer.Serialize(translators));
    }
}