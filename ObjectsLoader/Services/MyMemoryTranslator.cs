using DetectLanguage;
using Newtonsoft.Json;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Services;

public class MyMemoryTranslator : IDisposable
{
    private const string CachePath = "../../../lib/TranslationCache.json";
    private const string Target = "ru";
    private readonly HttpClientWrapper client;
    private readonly List<TranslationCacheJsonElement> cache;
    private readonly DetectLanguageClient identifier;
    
    public MyMemoryTranslator(HttpClientWrapper client)
    {
        this.client = client;
        var json = File.ReadAllText(CachePath);
        cache = JsonConvert.DeserializeObject<List<TranslationCacheJsonElement>>(json)!;
        identifier = new DetectLanguageClient("37ff2a1bbd15c8bac944a05bb3db817c");
    }

    public async Task<string?> Translate(string text)
    {
        var translation = cache.FirstOrDefault(x => x.SourceValue == text);
        if (translation is not null)
        {
            return translation.TargetValue;
        }
        
        var language = (await identifier.DetectAsync(text)).FirstOrDefault(x => x.reliable)?.language;
        if (language is null)
        {
            return null;
        }
        
        var jsonString = await client.GetMyMemoryJson(text, language, Target);
        if (jsonString is null)
        {
            return null;
        }
        var result = JsonConvert.DeserializeObject<MyMemoryJsonElement>(jsonString)!.ResponseData["translatedText"];
        
        cache.Add(new TranslationCacheJsonElement
        {
            SourceValue = text,
            TargetValue = result
        }); 
        
        return result;
    }
    
    private void SaveCache()
    {
        var json = JsonConvert.SerializeObject(cache);
        File.WriteAllText(CachePath, json);
    }

    public void Dispose()
    {
        SaveCache();
        GC.SuppressFinalize(this);
    }
}