using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Clients.Impl;

public class NominatimClient : INominatimClient
{
    private const string Url = "https://nominatim.openstreetmap.org/reverse?format=json&lat={0}&lon={1}";
    private readonly ILogger<NominatimClient> logger;
    private readonly HttpClient client = new();
    
    public NominatimClient(ILogger<NominatimClient> logger)
    {
        this.logger = logger;
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
        this.logger.LogInformation("NominatimClient initialized with url: '{Url}'", Url);
    }
    
    public async Task<string?> Fetch(string key, double latitude, double longitude)
    {
        logger.LogInformation("Fetching data from nominatim API with key: {Key}, latitude: {Lat}, longitude: {Lon}", key, latitude, longitude);
        
        var stringLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var stringLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        var query = string.Format(Url, stringLatitude, stringLongitude);
        
        logger.LogInformation("Nominatim query: '{Query}' built, sending request", query);
        
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
        
        logger.LogInformation("Parsing response data");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<NominatimJsonElement>(stringResponse);
        jsonElement!.Address.TryGetValue(key, out var result);

        if (result is not null)
        {
            logger.LogInformation("Found value: {Result} by key: {Key} in response data", result, key);
            return result;
        }
        
        logger.LogInformation("Can't find key: {Key} in response data, returning null", key);
        return null;
    }
}