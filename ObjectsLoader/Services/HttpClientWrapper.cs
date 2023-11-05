using System.Globalization;
using System.Net;

namespace ObjectsLoader.Services;

public class HttpClientWrapper
{
    private const string MyMemoryUrl = "https://api.mymemory.translated.net/get";
    private readonly HttpClient client = new();
    
    public async Task<string?> GetMyMemoryJson(string text, string source, string target)
    {
        var uri = $"{MyMemoryUrl}?q={text}&langpair={source}|{target}";
        var response = await client.GetAsync(uri);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
}