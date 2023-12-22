using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Clients.Impl;

public class NominatimClient : ClientBase, INominatimClient
{
    private const string Url = "https://nominatim.openstreetmap.org/reverse?format=json&lat={0}&lon={1}";
    
    public NominatimClient(ILogger<NominatimClient> logger) : base(logger)
    {
        Client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
        Logger.LogInformation("NominatimClient initialized with url: '{Url}'", Url);
    }
    
    public async Task<string?> Fetch(string key, double latitude, double longitude)
    {
        Logger.LogInformation("Fetching data from nominatim API with key: {Key}, latitude: {Lat}, longitude: {Lon}", key, latitude, longitude);
        
        var stringLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var stringLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        var query = string.Format(Url, stringLatitude, stringLongitude);
        
        Logger.LogInformation("Nominatim query: '{Query}' built, sending request", query);
        
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
        
        Logger.LogInformation("Parsing response data");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<NominatimJsonElement>(stringResponse);
        jsonElement!.Address.TryGetValue(key, out var result);

        if (result is not null)
        {
            Logger.LogInformation("Found value: {Result} by key: {Key} in response data", result, key);
            return result;
        }
        
        Logger.LogInformation("Can't find key: {Key} in response data, returning null", key);
        return null;
    }
}