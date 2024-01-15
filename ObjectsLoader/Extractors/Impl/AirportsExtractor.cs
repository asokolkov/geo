using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors.Impl;

public class AirportsExtractor : IExtractor<Airport>
{
    private const string Query = "[out:json];nwr[aeroway=aerodrome][iata][name];out center 5;";
    
    private readonly ILogger<AirportsExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly INominatimClient nominatimClient;
    private readonly ITranslatorClient translatorClient;
    private readonly ITimezoneManager timezoneManager;

    public AirportsExtractor(ILogger<AirportsExtractor> logger, IOsmClient osmClient, INominatimClient nominatimClient, ITranslatorClient translatorClient, ITimezoneManager timezoneManager)
    {
        this.logger = logger;
        this.osmClient = osmClient;
        this.nominatimClient = nominatimClient;
        this.translatorClient = translatorClient;
        this.timezoneManager = timezoneManager;
        logger.LogInformation("AirportsExtractor initialized with osm query: '{Query}'", Query);
    }
    
    public async Task<List<Airport>> Extract()
    {
        logger.LogInformation("Extracting airports, fetching from osm by query");
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            logger.LogInformation("Fetching failed, returning empty list");
            return new List<Airport>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
        
        logger.LogInformation("Parsing fetched airports");
        var result = new List<Airport>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var latitude = element.Latitude;
            var longitude = element.Longitude;
            var iataEn = element.Tags["iata"];
            var name = element.Tags["name"];
            element.Tags.TryGetValue("iata:ru", out var iataRu);
            element.Tags.TryGetValue("timezone", out var jsonTimezone);
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            element.Tags.TryGetValue("addr:city", out var jsonCity);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }

            var utcOffset = jsonTimezone is null 
                ? timezoneManager.GetUtcOffset(latitude, longitude)
                : timezoneManager.GetUtcOffset(jsonTimezone);

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

            var airport = new Airport
            {
                Osm = osmId,
                Latitude = latitude,
                Longitude = longitude,
                NameEn = name,
                NameRu = nameRu,
                IataEn = iataEn,
                IataRu = iataRu,
                UtcOffset = utcOffset,
                CityOsmId = cityOsmId
            };
            result.Add(airport);
        }
        
        logger.LogInformation("Returning extracted airports");
        return result;
    }
}