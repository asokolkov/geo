using Newtonsoft.Json;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors;

public class CitiesExtractor
{
    private const string Query = "[out:json];nwr[place=city][name];out;";
    
    private readonly HttpClientWrapper client;
    private readonly MyMemoryTranslator translator;
    private readonly TimezoneManager timezoneManager;

    public CitiesExtractor(HttpClientWrapper client, MyMemoryTranslator translator, TimezoneManager timezoneManager)
    {
        this.client = client;
        this.translator = translator;
        this.timezoneManager = timezoneManager;
    }
    
    public async Task<List<City>> Extract()
    {
        var jsonString = await client.GetOsmJson(Query);
        if (jsonString is null)
        {
            return new List<City>();
        }
        var jsonRoot = JsonConvert.DeserializeObject<OsmJsonRoot>(jsonString);
        
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
            
            var nameRu = jsonNameRu ?? await translator.Translate(name);
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
                var nominatimJson = await client.GetNominatimJson(latitude, longitude);
                if (nominatimJson is null)
                {
                    continue;
                }
                var jsonElement = JsonConvert.DeserializeObject<NominatimJsonElement>(nominatimJson);
                
                jsonElement!.Address.TryGetValue("ISO3166-2-lvl4", out var jsonNominatimRegionCode);
                regionIso = jsonNominatimRegionCode;
            }
            if (regionIso is null)
            {
                continue;
            }
            
            result.Add(new City
            {
                Id = Guid.NewGuid(),
                Latitude = latitude,
                Longitude = longitude,
                OsmId = osmId,
                NameRu = nameRu,
                Timezone = timezone,
                RegionIsoCode = regionIso
            });
        }
        
        return result;
    }
}