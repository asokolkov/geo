using System.Text.Json.Serialization;

namespace Geo.Api.V1.Countries.Models;

public class CountryDto
{
    public CountryDto(CountryCodeDto code, CountryNameDto name, CountryPhoneDto phone, CountryGeometryDto geometry,
        string osm, bool needToUpdate, DateTimeOffset lastUpdate)
    {
        Code = code;
        Name = name;
        Phone = phone;
        Osm = osm;
        NeedToUpdate = needToUpdate;
        LastUpdate = lastUpdate;
        Geometry = geometry;
    }

    [JsonPropertyName("code")] public CountryCodeDto Code { get; }

    [JsonPropertyName("name")] public CountryNameDto Name { get; }

    [JsonPropertyName("geometry")] public CountryGeometryDto Geometry { get; }

    [JsonPropertyName("phone")] public CountryPhoneDto Phone { get; }

    [JsonPropertyName("osm")] public string Osm { get; }

    [JsonPropertyName("need_to_update")] public bool NeedToUpdate { get; }

    [JsonPropertyName("last_update")] public DateTimeOffset LastUpdate { get; }
};