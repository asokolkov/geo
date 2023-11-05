using Newtonsoft.Json;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Lib;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class CountriesExtractor : IExtractor<Country>
{
    private const string Query = "[out:json];rel[admin_level=2][boundary=administrative][name][\"ISO3166-1\"][\"ISO3166-1:alpha2\"][\"ISO3166-1:alpha3\"];out ids tags 1;";
    
    private readonly IOsmClient osmClient;
    private readonly ITranslatorClient translatorClient;
    
    public CountriesExtractor(IOsmClient osmClient, ITranslatorClient translatorClient)
    {
        this.osmClient = osmClient;
        this.translatorClient = translatorClient;
    }
    
    public async Task<List<Country>> Extract()
    {
        var jsonString = await osmClient.Fetch(Query);
        if (jsonString is null)
        {
            return new List<Country>();
        }
        var jsonRoot = JsonConvert.DeserializeObject<OsmJsonRoot>(jsonString);
        
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

            result.Add(new Country
            {
                Id = Guid.NewGuid(),
                OsmId = osmId,
                Iso2 = iso2,
                Iso3 = iso3,
                NameRu = nameRu,
                PhoneCode = CountriesPhones.Codes[iso2],
                PhoneMask = CountriesPhones.Masks[iso2]
            });
        }
        
        return result;
    }
}