using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class RegionsExtractor : IExtractor<Region>
{
    private const string Query = "[out:json];rel[admin_level=4][boundary=administrative][name][\"ISO3166-2\"];out ids tags;";
    
    private readonly ILogger<RegionsExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly ITranslatorClient translatorClient;

    public RegionsExtractor(ILogger<RegionsExtractor> logger, IOsmClient osmClient, ITranslatorClient translatorClient)
    {
        this.logger = logger;
        this.osmClient = osmClient;
        this.translatorClient = translatorClient;
        logger.LogInformation("RegionsExtractor initialized with osm query: '{Query}'", Query);
    }
    
    public async Task<List<Region>> Extract()
    {
        logger.LogInformation("Extracting regions, fetching from osm by query");
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            logger.LogInformation("Fetching failed, returning empty list");
            return new List<Region>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString, new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });

        logger.LogInformation("Parsing fetched regions");
        var result = new List<Region>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var name = element.Tags["name"];
            element.Tags.TryGetValue("ISO3166-2", out var isoCode); 
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            element.Tags.TryGetValue("name:en", out var jsonNameEn);

            if (isoCode is null)
            {
                continue;
            }
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }

            var region = new Region
            {
                Osm = osmId,
                NameEn = jsonNameEn ?? name,
                NameRu = nameRu,
                CountryIso2Code = isoCode.Split("-").First(),
                Iso = isoCode
            };
            result.Add(region);
            logger.LogInformation("Parsed region with osm id: {OsmId}, russian name: {NameRu}", osmId, nameRu);
        }
        
        logger.LogInformation("Returning extracted regions");
        return result;
    }
}