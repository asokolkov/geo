using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors.Impl;

public class CitiesExtractor : IExtractor<City>
{
    private const string Query = "[out:json];nwr[place=city][name][\"is_in:iso_3166_2\"];out;";
    
    private readonly ILogger<CitiesExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly ITranslatorClient translatorClient;
    private readonly ITimezoneManager timezoneManager;

    public CitiesExtractor(ILogger<CitiesExtractor> logger, IOsmClient osmClient, ITranslatorClient translatorClient, ITimezoneManager timezoneManager)
    {
        this.logger = logger;
        this.osmClient = osmClient;
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
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString, new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
        
        logger.LogInformation("Parsing fetched cities");
        var result = new List<City>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var latitude = element.Latitude;
            var longitude = element.Longitude;
            var name = element.Tags["name"];
            var regionIso = element.Tags["is_in:iso_3166_2"];
            element.Tags.TryGetValue("timezone", out var jsonTimezone);
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            element.Tags.TryGetValue("name:en", out var jsonNameEn);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }
            
            var utcOffset = jsonTimezone is null 
                ? timezoneManager.GetUtcOffset(latitude, longitude)
                : timezoneManager.GetUtcOffset(jsonTimezone);

            var city = new City
            {
                Latitude = latitude,
                Longitude = longitude,
                Osm = osmId,
                NameRu = nameRu,
                NameEn = jsonNameEn ?? name,
                UtcOffset = utcOffset,
                RegionIsoCode = regionIso
            };
            result.Add(city);
        }
        
        logger.LogInformation("Returning extracted cities");
        return result;
    }
}