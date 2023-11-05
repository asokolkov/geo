using Newtonsoft.Json;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.Services;

namespace ObjectsLoader.Extractors;

public class RegionsExtractor
{
    private const string Query = "[out:json];rel[admin_level=4][boundary=administrative][name][\"ISO3166-2\"];out ids tags 1;";
    
    private readonly OsmClient osmClient;
    private readonly TranslatorClient translatorClient;
    
    public RegionsExtractor(OsmClient osmClient, TranslatorClient translatorClient)
    {
        this.osmClient = osmClient;
        this.translatorClient = translatorClient;
    }
    
    public async Task<List<Region>> Extract()
    {
        var jsonString = await osmClient.Fetch(Query);
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
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name);
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