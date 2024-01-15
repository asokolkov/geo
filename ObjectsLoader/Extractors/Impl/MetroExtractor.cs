using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class MetroExtractor : IExtractor<Metro>
{
    private const string Query = "[out:json];nwr[station=subway][name][line];out center 5;";
    
    private readonly ILogger<MetroExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly INominatimClient nominatimClient;
    private readonly ITranslatorClient translatorClient;
    
    public MetroExtractor(ILogger<MetroExtractor> logger, INominatimClient nominatimClient, IOsmClient osmClient, ITranslatorClient translatorClient)
    {
        this.nominatimClient = nominatimClient;
        this.logger = logger;
        this.osmClient = osmClient;
        this.translatorClient = translatorClient;
        logger.LogInformation("MetroExtractor initialized with osm query: '{Query}'", Query);
    }
    
    public async Task<List<Metro>> Extract()
    {
        logger.LogInformation("Extracting metro stations, fetching from osm by query");
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            logger.LogInformation("Fetching failed, returning empty list");
            return new List<Metro>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
        
        logger.LogInformation("Parsing fetched metro stations");
        var result = new List<Metro>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var latitude = element.Latitude;
            var longitude = element.Longitude;
            var name = element.Tags["name"];
            var line = element.Tags["line"];
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }
            
            var lineRu = await translatorClient.Fetch(line, "ru");
            if (lineRu is null)
            {
                continue;
            }
            
            var city = await nominatimClient.Fetch("country_code", latitude, longitude);
            if (city is null)
            {
                continue;
            }
            
            var osmJson = await osmClient.Fetch($"[out:json];nwr[name=\"{city}\"];out center 1;");
            if (osmJson is null)
            {
                continue;
            }
            var osmJsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
            var cityOsmId = osmJsonRoot!.Elements.FirstOrDefault()?.OsmId.ToString();
            if (cityOsmId is null)
            {
                continue;
            }

            var metro = new Metro
            {
                Osm = osmId,
                Latitude = latitude,
                Longitude = longitude,
                StationNameEn = name,
                StationNameRu = nameRu,
                LineNameEn = line,
                LineNameRu = lineRu,
                CityOsmId = cityOsmId
            };
            result.Add(metro);
            logger.LogInformation("Parsed metro station with osm id: {OsmId}, latitude: {Lat}, longitude: {Lon}, station name: {NameRu}, line: {Line}, translated line: {LineRu}", osmId, latitude, longitude, nameRu, line, lineRu);
        }
        
        logger.LogInformation("Returning extracted metro stations");
        return result;
    }
}