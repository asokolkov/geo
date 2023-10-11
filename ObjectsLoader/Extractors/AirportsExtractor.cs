using Newtonsoft.Json;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors;

public class AirportsExtractor
{
    private const string Query = "[out:json];nwr[aeroway=aerodrome][iata][name];out center 1;";
    
    private readonly HttpClientWrapper client;
    private readonly MyMemoryTranslator translator;
    private readonly TimezoneManager timezoneManager;

    public AirportsExtractor(HttpClientWrapper client, MyMemoryTranslator translator, TimezoneManager timezoneManager)
    {
        this.client = client;
        this.translator = translator;
        this.timezoneManager = timezoneManager;
    }
    
    public async Task<List<Airport>> Extract()
    {
        var jsonString = await client.GetOsmJson(Query);
        if (jsonString is null)
        {
            return new List<Airport>();
        }
        var jsonRoot = JsonConvert.DeserializeObject<OsmJsonRoot>(jsonString);
        
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
            
            var nameRu = jsonNameRu ?? await translator.Translate(name);
            if (nameRu is null)
            {
                continue;
            }

            var timezone = jsonTimezone is null 
                ? timezoneManager.GetUtcTimezone(latitude, longitude)
                : timezoneManager.GetUtcTimezone(jsonTimezone);
            
            result.Add(new Airport
            {
                Id = Guid.NewGuid(),
                OsmId = osmId,
                Latitude = latitude,
                Longitude = longitude,
                NameRu = nameRu,
                IataEn = iataEn,
                IataRu = iataRu,
                Timezone = timezone
            });
        }
        
        return result;
    }
}