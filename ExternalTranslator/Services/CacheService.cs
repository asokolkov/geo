using System.Text.Json;
using ExternalTranslator.Enums;
using ExternalTranslator.Models;

namespace ExternalTranslator.Services;

public class CacheService
{
    private const string CachePath = "./Properties/TranslationCache.json";
    private readonly List<Translator> translators;
    
    public CacheService()
    {
        
        if (File.Exists(CachePath))
        {
            using var stream = File.OpenRead(CachePath);
            translators = JsonSerializer.Deserialize<List<Translator>>(stream)!;
        }
        else
        {
            translators = new List<Translator>();
        }
    }

    public Translator Get(string translatorId)
    {
        return translators.First(x => x.Id == translatorId);
    }

    public void Update(string translatorId, string text)
    {
        var translator = translators.First(x => x.Id == translatorId);
        foreach (var restriction in translator.Restrictions)
        {
            restriction.CurrentAmount += restriction.Type switch
            {
                RestrictionType.Chars => text.Length,
                RestrictionType.Queries => 1,
                RestrictionType.Bytes => text.Length * sizeof(char)
            };
        }
        translator.TimeCheckpoint ??= DateTimeOffset.Now;
        Save();
    }
    
    public void Reset(string translatorId)
    {
        var translator = translators.First(x => x.Id == translatorId);
        foreach (var restriction in translator.Restrictions)
        {
            restriction.CurrentAmount = 0;
        }
        translator.TimeCheckpoint = null;
        Save();
    }
    
    private void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(CachePath, JsonSerializer.Serialize(translators, options));
    }
}