using System.Text.Json.Serialization;

namespace Geo.Api.Client.Internal.Cities.Models;

internal sealed class CreateCityDto
{
    public CreateCityDto(int cityId, string name, string iataEn, double latitude, double longitude, string timezone,
        string osm)
    {
        CityId = cityId;
        Name = name;
        IataEn = iataEn;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
    }

    [JsonPropertyName("city_id")]
    public int CityId { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("iata_en")]
    public string IataEn { get; }

    [JsonPropertyName("iata_ru")]
    public string? IataRu { get; init; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; }

    [JsonPropertyName("osm")]
    public string Osm { get; }
}