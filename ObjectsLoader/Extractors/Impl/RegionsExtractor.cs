using Newtonsoft.Json;
using ObjectsLoader.Clients;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;

namespace ObjectsLoader.Extractors.Impl;

public class RegionsExtractor : IExtractor<Region>
{
    private const string Query = "[out:json];rel[admin_level=4][boundary=administrative][name][\"ISO3166-2\"];out ids tags 1;";
    
    private readonly IOsmClient osmClient;
    private readonly ITranslatorClient translatorClient;
    
    public RegionsExtractor(IOsmClient osmClient, ITranslatorClient translatorClient)
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
            
            var nameRu = jsonNameRu ?? await translatorClient.Fetch(name, "ru");
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