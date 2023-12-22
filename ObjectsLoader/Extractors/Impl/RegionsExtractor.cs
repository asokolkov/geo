﻿using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class RegionsExtractor : IExtractor<Region>
{
    private const string Query = "[out:json];rel[admin_level=4][boundary=administrative][name][\"ISO3166-2\"];out ids tags 1;";
    
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
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);

        logger.LogInformation("Parsing fetched regions");
        var result = new List<Region>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var iso = element.Tags["ISO3166-2"];
            var name = element.Tags["name"];
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }

            var region = new Region
            {
                Id = Guid.NewGuid(),
                OsmId = osmId,
                NameRu = nameRu,
                IsoCode = iso
            };
            result.Add(region);
            logger.LogInformation("Parsed region with id: {Id}, osm id: {OsmId}, russian name: {NameRu}, iso code: {IsoCode}", region.Id, osmId, nameRu, iso);
        }
        
        logger.LogInformation("Returning extracted regions");
        return result;
    }
}