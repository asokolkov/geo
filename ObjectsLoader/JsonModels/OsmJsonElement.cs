using System.Text.Json.Serialization;
using ObjectsLoader.Extensions;

namespace ObjectsLoader.JsonModels;

internal sealed class OsmJsonElement
{
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("id")] public required string OsmId { get; init; }
    [JsonPropertyName("lat")] public double Latitude { get; init; }
    [JsonPropertyName("lon")] public double Longitude { get; init; }
    [JsonPropertyName("tags")] public required Dictionary<string, string> Tags { get; init; }
}