using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Lib;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class CountriesExtractor : IExtractor<Country>
{
    private const string Query = "[out:json];rel[admin_level=2][boundary=administrative][name][\"ISO3166-1:alpha2\"][\"ISO3166-1:alpha3\"];out ids tags;";
    
    private readonly ILogger<CountriesExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly ITranslatorClient translatorClient;

    public CountriesExtractor(ILogger<CountriesExtractor> logger, IOsmClient osmClient, ITranslatorClient translatorClient)
    {
        this.logger = logger;
        this.osmClient = osmClient;
        this.translatorClient = translatorClient;
        logger.LogInformation("CountriesExtractor initialized with osm query: '{Query}'", Query);
    }
    
    public async Task<List<Country>> Extract()
    {
        logger.LogInformation("Extracting countries, fetching from osm by query");
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            logger.LogInformation("Fetching failed, returning empty list");
            return new List<Country>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
        
        logger.LogInformation("Parsing fetched countries");
        var result = new List<Country>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var iso2 = element.Tags["ISO3166-1:alpha2"];
            var iso3 = element.Tags["ISO3166-1:alpha3"];
            var name = element.Tags["name"];
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            element.Tags.TryGetValue("name:en", out var jsonNameEn);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }

            var country = new Country
            {
                Osm = osmId,
                Iso3116Alpha2 = iso2,
                Iso3116Alpha3 = iso3,
                NameEn = jsonNameEn ?? name,
                NameRu = nameRu,
                PhoneCode = CountriesPhones.Codes[iso2],
                PhoneMask = CountriesPhones.Masks[iso2]
            };
            result.Add(country);
            logger.LogInformation("Parsed country with osm id: {OsmId}, iso code 2: {Iso2}, iso code 3: {Iso3}, russian name: {NameRu}, phone code: {PhoneCode}, phone mask: {PhoneMask}", osmId, iso2, iso3, nameRu, country.PhoneCode, country.PhoneMask);
        }
        
        logger.LogInformation("Returning extracted countries");
        return result;
    }
}