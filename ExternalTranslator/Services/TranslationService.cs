using ExternalTranslator.Services.Translators;

namespace ExternalTranslator.Services;

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
            await translator.TryResetModel();
            if (translator.CanTranslate(text))
            {
                return await translator.Translate(text, source, target);
            }
        }
        return null;
    }
}