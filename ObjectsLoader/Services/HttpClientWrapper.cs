using System.Globalization;
using System.Net;

namespace ObjectsLoader.Services;

public class HttpClientWrapper
{
    private const string OsmUrl = "https://overpass-api.de/api/interpreter";
    private const string MyMemoryUrl = "https://api.mymemory.translated.net/get";
    private const string NominatimUrl = "https://nominatim.openstreetmap.org/reverse";
    private readonly HttpClient client = new();

    public HttpClientWrapper()
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
    }

    public async Task<string?> GetOsmJson(string query)
    {
        var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("data", query) });
        var response = await client.PostAsync(OsmUrl, content);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
    
    public async Task<string?> GetMyMemoryJson(string text, string source, string target)
    {
        var uri = $"{MyMemoryUrl}?q={text}&langpair={source}|{target}";
        var response = await client.GetAsync(uri);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
    
    public async Task<string?> GetNominatimJson(double latitude, double longitude)
    {
        var stringLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var stringLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        var uri = $"{NominatimUrl}?format=json&lat={stringLatitude}&lon={stringLongitude}";
        var response = await client.GetAsync(uri);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
}