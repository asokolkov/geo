using System.Net;
using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public class OsmClient : ClientBase, IOsmClient
{
    private const string Url = "https://overpass-api.de/api/interpreter?data={0}";
    private readonly ILogger<OsmClient> logger;

    public OsmClient(ILogger<OsmClient> logger)
    {
        this.logger = logger;
    }
    
    public async Task<string?> Fetch(string data)
    {
        var query = string.Format(Url, data);
        var response = await SendRequest(query);
        if (response is null)
        {
            logger.LogInformation("{{method=\"fetch\" status=\"fail\" msg=\"Bad response\"}}");
            return null;
        }
        logger.LogInformation("{{method=\"fetch\" http_method=\"{Method}\" uri=\"{Uri}\" status_code=\"{Code}\" msg=\"Got response\"}}", response.RequestMessage?.Method, response.RequestMessage?.RequestUri, response.StatusCode);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync();
        }
        logger.LogInformation("{{method=\"fetch\" status=\"fail\" msg=\"Bad response status code\"}}");
        return null;
    }
}