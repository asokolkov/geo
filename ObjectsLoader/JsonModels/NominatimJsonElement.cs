using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class NominatimJsonElement
{
    [JsonPropertyName("address")] public required Dictionary<string, string> Address { get; init; }
}