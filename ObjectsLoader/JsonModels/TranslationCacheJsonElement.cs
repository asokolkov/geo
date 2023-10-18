using Newtonsoft.Json;

namespace ObjectsLoader.JsonModels;

public class TranslationCacheJsonElement
{
    [JsonProperty("sourceValue")] public string SourceValue { get; init; } = null!;
    [JsonProperty("targetValue")] public string TargetValue { get; init; } = null!;
}