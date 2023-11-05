using System.Net;

namespace ObjectsLoader.Clients;

public class TranslatorClient
{
    private const string Url = "https://localhost:7001/translate?text={0}&target=ru&source={1}";
    private readonly HttpClient client = new();

    public async Task<string?> Fetch(string text, string? source = null)
    {
        var query = string.Format(Url, text, source);
        var response = await client.GetAsync(query);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
}