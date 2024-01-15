using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Geo.Api.V1;

[PublicAPI]
public sealed record SearchResultElementDto(
    [property: JsonPropertyName("object_type")]
    string ObjectType,
    [property: JsonPropertyName("object_id")]
    string ObjectId,
    [property: JsonPropertyName("object_name")]
    string ObjectName,
    [property: JsonPropertyName("country_id")]
    string CountryId,
    [property: JsonPropertyName("country_name")]
    string CountryName)
{
    [JsonPropertyName("region_id")] public string? RegionId { get; init; }

    [JsonPropertyName("region_name")] public string? RegionName { get; init; }
}