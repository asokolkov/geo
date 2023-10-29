using ExternalTranslator.Translators;

namespace ExternalTranslator.Services.Impl;

public class TranslationService : ITranslationService
{
    private readonly List<ITranslatorClient> translators;
    
    public TranslationService(IMyMemoryClient myMemoryClient, IYandexClient yandexClient)
    {
        translators = new List<ITranslatorClient>
        {
            myMemoryClient, yandexClient
        };
    }
    
    public async Task<string?> Translate(string text, string? source, string target)
    {
        foreach (var translator in translators)
        {
            await translator.TryResetRestrictions();
            var translation = translator.CanTranslate(text) 
                ? await translator.Translate(text, source, target) 
                : null;
            if (translation is not null)
            {
                return translation;
            }
        }
        return null;
    }
}