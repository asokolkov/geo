using Newtonsoft.Json;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors;

public class RegionsExtractor
{
    private const string Query = "[out:json];rel[admin_level=4][boundary=administrative][name][\"ISO3166-2\"];out ids tags;";
    
    private readonly HttpClientWrapper client;
    private readonly MyMemoryTranslator translator;
    
    public RegionsExtractor(HttpClientWrapper client, MyMemoryTranslator translator)
    {
        this.client = client;
        this.translator = translator;
    }
    
    public async Task<List<Region>> Extract()
    {
        var jsonString = await client.GetOsmJson(Query);
        if (jsonString is null)
        {
            return new List<Region>();
        }
        var jsonRoot = JsonConvert.DeserializeObject<OsmJsonRoot>(jsonString);

        var result = new List<Region>();
        foreach (var element in jsonRoot!.Elements)
        {
            var osmId = element.OsmId;
            var iso = element.Tags["ISO3166-2"];
            var name = element.Tags["name"];
            element.Tags.TryGetValue("name:ru", out var jsonNameRu);
            
            var nameRu = jsonNameRu ?? await translator.Translate(name);
            if (nameRu is null)
            {
                continue;
            }
            
            result.Add(new Region
            {
                Id = Guid.NewGuid(),
                OsmId = osmId,
                NameRu = nameRu,
                IsoCode = iso
            });
        }
        
        return result;
    }
}