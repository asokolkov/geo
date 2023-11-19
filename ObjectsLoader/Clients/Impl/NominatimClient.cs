using System.Globalization;
using System.Net;
using System.Text.Json;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Clients.Impl;

public class NominatimClient : INominatimClient
{
    private const string Url = "https://nominatim.openstreetmap.org/reverse?format=json&lat={0}&lon={1}";
    private readonly HttpClient client = new();
    
    public NominatimClient()
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
    }
    
    public async Task<string?> Fetch(string key, double latitude, double longitude)
    {
        var stringLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var stringLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        var query = string.Format(Url, stringLatitude, stringLongitude);
        var response = await client.GetAsync(query);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        var stringResponse = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<NominatimJsonElement>(stringResponse);
        jsonElement!.Address.TryGetValue(key, out var result);
        return result;
    }
}