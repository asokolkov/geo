using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors.Impl;

public class RailwaysExtractor : IExtractor<Railway>
{
    private const string Query = "[out:json];nwr[building=train_station][name];out center 1;";
    
    private readonly ILogger<RailwaysExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly INominatimClient nominatimClient;
    private readonly ITranslatorClient translatorClient;
    private readonly ITimezoneManager timezoneManager;

    public RailwaysExtractor(ILogger<RailwaysExtractor> logger, IOsmClient osmClient, INominatimClient nominatimClient, ITranslatorClient translatorClient, ITimezoneManager timezoneManager)
    {
        this.logger = logger;
        this.osmClient = osmClient;
        this.nominatimClient = nominatimClient;
        this.translatorClient = translatorClient;
        this.timezoneManager = timezoneManager;
        logger.LogInformation("RailwaysExtractor initialized with osm query: '{Query}'", Query);
    }
    
    public async Task<List<Railway>> Extract()
    {
        logger.LogInformation("Extracting railways, fetching from osm by query");
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            logger.LogInformation("Fetching failed, returning empty list");
            return new List<Railway>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
        
        logger.LogInformation("Parsing fetched railways");
        var result = new List<Railway>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var latitude = element.Latitude;
            var longitude = element.Longitude;
            var name = element.Tags["name"];
            element.Tags.TryGetValue("timezone", out var jsonTimezone);
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            element.Tags.TryGetValue("addr:city", out var jsonCity);
            element.Tags.TryGetValue("uic_ref", out var uic);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }

            var timezone = jsonTimezone is null 
                ? timezoneManager.GetUtcTimezone(latitude, longitude)
                : timezoneManager.GetUtcTimezone(jsonTimezone);

            var isMain = jsonCity is not null;
            
            var city = jsonCity;
            if (city is null)
            {
                var cityName = await nominatimClient.Fetch("country_code", latitude, longitude);
                if (cityName is null)
                {
                    continue;
                }
                city = cityName;
            }
            
            var osmJson = await osmClient.Fetch($"[out:json];nwr[name=\"{city}\"];out center 1;");
            if (osmJson is null)
            {
                continue;
            }
            var osmJsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
            var cityOsmId = osmJsonRoot!.Elements.FirstOrDefault()?.OsmId;
            if (cityOsmId is null)
            {
                continue;
            }

            var railway = new Railway
            {
                Osm = osmId,
                Latitude = latitude,
                Longitude = longitude,
                Name = nameRu,
                IsMain = isMain,
                Express3Code = uic ?? "",
                Timezone = timezone,
                // CityOsmId = (int)cityOsmId
            };
            result.Add(railway);
            logger.LogInformation("Parsed railway with osm id: {OsmId}, latitude: {Lat}, longitude: {Lon}, russian name: {NameRu}, station main: {IsMain}, rzd: {Rzd}, timezone: {Timezone}", osmId, latitude, longitude, nameRu, isMain, railway.Express3Code, timezone);
        }
        
        logger.LogInformation("Returning extracted railways");
        return result;
    }
}