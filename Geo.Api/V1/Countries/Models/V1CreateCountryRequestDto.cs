namespace Geo.Api.V1.Countries.Models;

using System.Text.Json.Serialization;
using JetBrains.Annotations;

[PublicAPI]
public sealed class V1CreateCountryRequestDto
{
    [JsonPropertyName("name")] public required string Name { get; init; }

    [JsonPropertyName("iso3116Alpha2")] public required string Iso3116Alpha2Code { get; init; }
    
    [JsonPropertyName("iso3166Alpha3")] public required string Iso3166Alpha3Code { get; init; }
    
    [JsonPropertyName("phoneCode")] public required string PhoneCode { get; init; }
    
    [JsonPropertyName("phoneMask")] public required string PhoneMask { get; init; }
    
    [JsonPropertyName("osm")] public required string Osm { get; init; }
}