using System.Net;
using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public class OsmClient : ClientBase, IOsmClient
{
    private const string Url = "https://overpass-api.de/api/interpreter?data={0}";

    public OsmClient(ILogger<OsmClient> logger) : base(logger)
    {
        Logger.LogInformation("OsmClient initialized with url: '{Url}'", Url);
    }
    
    public async Task<string?> Fetch(string data)
    {
        Logger.LogInformation("Fetching data from osm API");

        var query = string.Format(Url, data);
        
        Logger.LogInformation("Osm query: '{Query}' built, sending request", query);
        
        var response = await SendRequest(query);
        if (response is null)
        {
            Logger.LogInformation("Failed to send request, returning null");
            return null;
        }
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Logger.LogInformation("Response status code is not 200, returning null");
            return null;
        }
        
        Logger.LogInformation("Response status code is 200, returning response content");
        return await response.Content.ReadAsStringAsync();
    }
}