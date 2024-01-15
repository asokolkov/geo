using System.Text.Json.Serialization;

namespace Geo.Api.V1.Countries.Models;

internal sealed class CountryDtoWithId : CountryDto
{
    public CountryDtoWithId(int id, CountryCodeDto code, CountryNameDto name, CountryPhoneDto phone,
        CountryGeometryDto geometry, string osm, bool needToUpdate, DateTimeOffset lastUpdate) : base(code, name, phone,
        geometry, osm, needToUpdate, lastUpdate)
    {
        Id = id;
    }

    [JsonPropertyName("id")] public int Id { get; }
}