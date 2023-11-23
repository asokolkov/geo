using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Clients.Impl;

public class NominatimClient : ClientBase, INominatimClient
{
    private const string Url = "https://nominatim.openstreetmap.org/reverse?format=json&lat={0}&lon={1}";
    private readonly ILogger<NominatimClient> logger;
    
    public NominatimClient(ILogger<NominatimClient> logger)
    {
        this.logger = logger;
        Client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
        logger.LogInformation("{{method=\"nominatim_client_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }
    
    public async Task<string?> Fetch(string key, double latitude, double longitude)
    {
        var stringLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var stringLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        var query = string.Format(Url, stringLatitude, stringLongitude);
        var response = await SendRequest(query);
        if (response is null)
        {
            logger.LogInformation("{{method=\"fetch\" status=\"fail\" msg=\"Bad response\"}}");
            return null;
        }
        logger.LogInformation("{{method=\"fetch\" http_method=\"{Method}\" uri=\"{Uri}\" status_code=\"{Code}\" msg=\"Got response\"}}", response.RequestMessage?.Method, response.RequestMessage?.RequestUri, response.StatusCode);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation("{{method=\"fetch\" status=\"fail\" msg=\"Bad response status code\"}}");
            return null;
        }
        var stringResponse = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<NominatimJsonElement>(stringResponse);
        jsonElement!.Address.TryGetValue(key, out var result);
        logger.LogInformation("{{method=\"fetch\" status=\"{Status}\" msg=\"For key {Key}, latitude {StringLatitude} and longitude {StringLongitude} found {Result}\"}}", result is null ? "fail" : "success", key, stringLatitude, stringLongitude, result);
        return result;
    }
}