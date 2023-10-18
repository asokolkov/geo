using Newtonsoft.Json;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors;

public class CountriesExtractor
{
    private const string Query = "[out:json];rel[admin_level=2][boundary=administrative][name][\"ISO3166-1\"][\"ISO3166-1:alpha2\"][\"ISO3166-1:alpha3\"];out ids tags 1;";
    private const string CountriesPhoneMasksPath = "../../../lib/CountriesPhoneMasks.json";
    private const string CountriesPhoneCodesPath = "../../../lib/CountriesPhoneCodes.json";
    
    private readonly HttpClientWrapper client;
    private readonly MyMemoryTranslator translator;
    
    public CountriesExtractor(HttpClientWrapper client, MyMemoryTranslator translator)
    {
        this.client = client;
        this.translator = translator;
    }
    
    public async Task<List<Country>> Extract()
    {
        var jsonString = await client.GetOsmJson(Query);
        if (jsonString is null)
        {
            return new List<Country>();
        }
        var jsonRoot = JsonConvert.DeserializeObject<OsmJsonRoot>(jsonString);
        
        var countriesPhoneCodesString = await File.ReadAllTextAsync(CountriesPhoneCodesPath);
        var countriesPhoneCodes = JsonConvert.DeserializeObject<Dictionary<string, string>>(countriesPhoneCodesString);
        var countriesPhoneMasksString = await File.ReadAllTextAsync(CountriesPhoneMasksPath);
        var countriesPhoneMasks = JsonConvert.DeserializeObject<Dictionary<string, string>>(countriesPhoneMasksString);
        
        var result = new List<Country>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var iso2 = element.Tags["ISO3166-1:alpha2"];
            var iso3 = element.Tags["ISO3166-1:alpha3"];
            var name = element.Tags["name"];
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            
            var nameRu = jsonNameRu ?? await translator.Translate(name);
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
                PhoneCode = countriesPhoneCodes![iso2],
                PhoneMask = countriesPhoneMasks![iso2]
            });
        }
        
        return result;
    }
}