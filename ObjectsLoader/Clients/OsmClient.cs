using System.Net;

namespace ObjectsLoader.Clients;

public class OsmClient
{
    private const string Url = "https://overpass-api.de/api/interpreter?data={0}";
    private readonly HttpClient client = new();
    
    public async Task<string?> Fetch(string data)
    {
        var query = string.Format(Url, data);
        var response = await client.GetAsync(query);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
}