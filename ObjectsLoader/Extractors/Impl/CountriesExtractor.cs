using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Lib;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class CountriesExtractor : IExtractor<Country>
{
    private const string Query = "[out:json];rel[admin_level=2][boundary=administrative][name][\"ISO3166-1\"][\"ISO3166-1:alpha2\"][\"ISO3166-1:alpha3\"];out ids tags 1;";
    
    private readonly ILogger<CountriesExtractor> logger;
    private readonly IOsmClient osmClient;
    private readonly ITranslatorClient translatorClient;

    public CountriesExtractor(ILogger<CountriesExtractor> logger, IOsmClient osmClient, ITranslatorClient translatorClient)
    {
        this.logger = logger;
        this.osmClient = osmClient;
        this.translatorClient = translatorClient;
        logger.LogInformation("{{method=\"countries_extractor_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }
    
    public async Task<List<Country>> Extract()
    {
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            return new List<Country>();
        }
        var jsonRoot = JsonSerializer.Deserialize<OsmJsonRoot>(jsonString);
        
        var result = new List<Country>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var iso2 = element.Tags["ISO3166-1:alpha2"];
            var iso3 = element.Tags["ISO3166-1:alpha3"];
            var name = element.Tags["name"];
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
            if (nameRu is null)
            {
                continue;
            }

            var country = new Country
            {
                Id = Guid.NewGuid(),
                OsmId = osmId,
                Iso2 = iso2,
                Iso3 = iso3,
                NameRu = nameRu,
                PhoneCode = CountriesPhones.Codes[iso2],
                PhoneMask = CountriesPhones.Masks[iso2]
            };
            result.Add(country);
            logger.LogInformation("{{method=\"extract\" id=\"{Id}\" osm_id=\"{OsmId}\" iso_2=\"{Iso2}\" iso_3=\"{Iso3}\" name_ru=\"{NameRu}\" phone_code=\"{PhoneCode}\" phone_mask=\"{PhoneMask}\" msg=\"Extracted country\"}}", country.Id, osmId, iso2, iso3, nameRu, country.PhoneCode, country.PhoneMask);
        }
        
        return result;
    }
}