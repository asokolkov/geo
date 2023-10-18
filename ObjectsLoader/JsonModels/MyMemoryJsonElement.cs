using Newtonsoft.Json;

namespace ObjectsLoader.JsonModels;

public class MyMemoryJsonElement
{
    [JsonProperty("responseData")] public Dictionary<string, string> ResponseData { get; init; } = null!;
}