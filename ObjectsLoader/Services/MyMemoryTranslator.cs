using DetectLanguage;
using Newtonsoft.Json;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Services;

public class MyMemoryTranslator : IDisposable
{
    private const string CachePath = "../../../Lib/TranslationCache.json";
    private const string Target = "ru";
    private readonly HttpClientWrapper client;
    private readonly Dictionary<string, string> cache;
    private readonly DetectLanguageClient identifier;
    
    public MyMemoryTranslator(HttpClientWrapper client)
    {
        this.client = client;
        var json = File.ReadAllText(CachePath);
        cache = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)!;
        identifier = new DetectLanguageClient("37ff2a1bbd15c8bac944a05bb3db817c");
    }

    public async Task<string?> Translate(string text)
    {
        cache.TryGetValue(text, out var translation);
        if (translation is not null)
        {
            return translation;
        }

        string language;
        try
        {
            var languageResponse = await identifier.DetectAsync(text);
            language = languageResponse.FirstOrDefault(x => x.reliable)?.language ?? "en";
        }
        catch (HttpRequestException e)
        {
            return null;
        }
        
        var jsonString = await client.GetMyMemoryJson(text, language, Target);
        if (jsonString is null)
        {
            return null;
        }
        
        var result = JsonConvert.DeserializeObject<MyMemoryJsonElement>(jsonString)?.ResponseData["translatedText"];
        if (result is null)
        {
            return null;
        }

        cache[text] = result;
        
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