using System.Text.Json.Serialization;

namespace Geo.Api.V1.Cities.Models;

internal sealed class CityDtoWithId : CityDto
{
    public CityDtoWithId(int id, CityCodeDto code, CityNameDto name, CityLocationComponentsDto locationComponents,
        CityGeometryDto geometry, string osm, bool needToUpdate, DateTimeOffset lastUpdate) : base(code, name,
        locationComponents, geometry, osm,
        needToUpdate, lastUpdate)
    {
        Id = id;
    }

    [JsonPropertyName("id")] public int Id { get; }
}