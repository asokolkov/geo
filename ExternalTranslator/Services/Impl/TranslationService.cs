using ExternalTranslator.Translators;

namespace ExternalTranslator.Services.Impl;

public class TranslationService : ITranslationService
{
    private readonly List<ITranslatorClient> translators;
    private readonly ILogger<TranslationService> logger;
    
    public TranslationService(IEnumerable<ITranslatorClient> translatorsClients, ILogger<TranslationService> logger)
    {
        translators = translatorsClients.ToList();
        this.logger = logger;
        logger.LogInformation("TranslationService initialized");
    }
    
    public async Task<string?> Translate(string text, string? source, string target)
    {
        if (source == target)
        {
            logger.LogInformation("Source language is equal to target language, returning passed text");
            return text;
        }
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