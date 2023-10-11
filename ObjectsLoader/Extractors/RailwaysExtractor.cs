using Newtonsoft.Json;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors;

public class RailwaysExtractor
{
    private const string Query = "[out:json];nwr[building=train_station][name];out center 1;";
    
    private readonly HttpClientWrapper client;
    private readonly MyMemoryTranslator translator;
    private readonly TimezoneManager timezoneManager;

    public RailwaysExtractor(HttpClientWrapper client, MyMemoryTranslator translator, TimezoneManager timezoneManager)
    {
        this.client = client;
        this.translator = translator;
        this.timezoneManager = timezoneManager;
    }
    
    public async Task<List<Railway>> Extract()
    {
        var jsonString = await client.GetOsmJson(Query);
        if (jsonString is null)
        {
            return new List<Railway>();
        }
        var jsonRoot = JsonConvert.DeserializeObject<OsmJsonRoot>(jsonString);
        
        var result = new List<Railway>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var latitude = element.Latitude;
            var longitude = element.Longitude;
            var name = element.Tags["name"];
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
            
            result.Add(new Railway
            {
                Id = Guid.NewGuid(),
                OsmId = osmId,
                Latitude = latitude,
                Longitude = longitude,
                NameRu = nameRu,
                IsMain = default,
                Rzd = default,
                Timezone = timezone
            });
        }
        
        return result;
    }
}