using DetectLanguage;
using ExternalTranslator.Models;

namespace ExternalTranslator.Services.Translators;

internal class MyMemoryTranslator : TranslatorBase
{
    private readonly DetectLanguageClient languageIdentifier;

    public MyMemoryTranslator(HttpClient client, CacheService cacheService, string detectLanguageApiKey) : base(client, cacheService)
    {
        languageIdentifier = new DetectLanguageClient(detectLanguageApiKey);
        Model = new Translator
        {
            Id = "MyMemory",
            Url = "https://api.mymemory.translated.net/get?q={0}&langpair={1}|{2}",
            CharsMax = 5000,
            CharsPeriod = TimeSpan.FromDays(1)
        };
    }

    public override async Task<string?> Translate(string text, string? source, string target)
    {
        var validSource = source ?? await IdentifySource(text);
        if (source == target)
        {
            return null;
        }
        var url = string.Format(Model.Url, text, validSource, target);
        var response = await SendQuery<MyMemoryJson>(url);
        var translation = response?.ResponseData.TranslatedText;
        if (translation is null)
        {
            return null;
        }

        CacheService.Update(Model.Id, text.Length);
        
        return translation;
    }
    
    private async Task<string> IdentifySource(string text)
    {
        var languageResponse = await languageIdentifier.DetectAsync(text);
        return languageResponse.FirstOrDefault(x => x.reliable)?.language ?? "en";
    }
}