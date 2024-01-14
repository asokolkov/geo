using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors.Impl;

public class CitiesExtractor : IExtractor<City>
{
    private const string Query = "[out:json];nwr[place=city][name];out 1;";
    
    private readonly ILogger<CitiesExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly INominatimClient nominatimClient;
    private readonly ITranslatorClient translatorClient;
    private readonly ITimezoneManager timezoneManager;

    public CitiesExtractor(ILogger<CitiesExtractor> logger, IOsmClient osmClient, INominatimClient nominatimClient, ITranslatorClient translatorClient, ITimezoneManager timezoneManager)
    {
        this.logger = logger;
        this.osmClient = osmClient;
        this.nominatimClient = nominatimClient;
        this.translatorClient = translatorClient;
        this.timezoneManager = timezoneManager;
        logger.LogInformation("CitiesExtractor initialized with osm query: '{Query}'", Query);
    }
    
    public async Task<List<City>> Extract()
    {
        logger.LogInformation("Extracting cities, fetching from osm by query");
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            logger.LogInformation("Fetching failed, returning empty list");
            return new List<City>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
        
        logger.LogInformation("Parsing fetched cities");
        var result = new List<City>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var latitude = element.Latitude;
            var longitude = element.Longitude;
            var name = element.Tags["name"];
            element.Tags.TryGetValue("timezone", out var jsonTimezone);
            element.Tags.TryGetValue("is_in:iso_3166_2", out var jsonRegionIso);
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }
            
            var timezone = jsonTimezone is null 
                ? timezoneManager.GetUtcTimezone(latitude, longitude)
                : timezoneManager.GetUtcTimezone(jsonTimezone);
            
            var regionIso = jsonRegionIso;
            if (regionIso is null)
            {
                var isoCode = await nominatimClient.Fetch("ISO3166-2-lvl4", latitude, longitude);
                if (isoCode is null)
                {
                    continue;
                }
                regionIso = isoCode;
            }

            var city = new City
            {
                Latitude = latitude,
                Longitude = longitude,
                Osm = osmId,
                Name = nameRu,
                Timezone = timezone,
                Iata = regionIso
            };
            result.Add(city);
            logger.LogInformation("Parsed city with latitude: {Lat}, longitude: {Lon}, osm id: {OsmId}, russian name: {NameRu}, timezone: {Timezone}, region iso code: {IsoCode}", latitude, longitude, osmId, nameRu, timezone, regionIso);
        }
        
        logger.LogInformation("Returning extracted cities");
        return result;
    }
}