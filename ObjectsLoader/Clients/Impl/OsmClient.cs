using System.Net;
using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public class OsmClient : IOsmClient
{
    private const string Url = "https://overpass-api.de/api/interpreter?data={0}";
    private readonly ILogger<OsmClient> logger;
    private readonly HttpClient client = new();

    public OsmClient(ILogger<OsmClient> logger)
    {
        this.logger = logger;
        this.logger.LogInformation("OsmClient initialized with url: '{Url}'", Url);
    }
    
    public async Task<string?> Fetch(string data)
    {
        logger.LogInformation("Fetching data from osm API");

        var query = string.Format(Url, data);
        
        logger.LogInformation("Osm query: '{Query}' built, sending request", query);
        
        HttpResponseMessage? response;
        try
        {
            response = await client.GetAsync(query);
            logger.LogInformation("Request sent without exceptions, returning response");
        }
        catch (HttpRequestException _)
        {
            logger.LogInformation("Got exception on request with query: '{Query}', returning null", query);
            response = null;
        }
        
        if (response is null)
        {
            logger.LogInformation("Failed to send request, returning null");
            return null;
        }
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("Response status code is not 200, returning null");
            return null;
        }
        
        logger.LogInformation("Response status code is 200, returning response content");
        return await response.Content.ReadAsStringAsync();
    }
}