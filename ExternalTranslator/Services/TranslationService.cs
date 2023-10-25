using ExternalTranslator.Services.Translators;

namespace ExternalTranslator.Services;

public class TranslationService
{
    private readonly List<TranslatorBase> translators;

    public TranslationService(string detectLanguageApiKey)
    {
        var client = new HttpClient();
        var cache = new CacheService();
        translators = new List<TranslatorBase>
        {
            new MyMemoryTranslator(client, cache, detectLanguageApiKey)
        };
    }
    
    public async Task<string?> Translate(string text, string? source, string target)
    {
        foreach (var translator in translators)
        {
            translator.TryUpdatePeriods();
            if (translator.CanTranslate(text))
            {
                return await translator.Translate(text, source, target);
            }
        }
        return null;
    }
}