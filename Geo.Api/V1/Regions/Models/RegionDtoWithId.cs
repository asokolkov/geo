using System.Text.Json.Serialization;

namespace Geo.Api.V1.Regions.Models;

internal sealed class RegionDtoWithId : RegionDto
{
    public RegionDtoWithId(int id, RegionNameDto name, RegionLocationComponentsDto locationComponents,
        RegionGeometryDto geometry, string osm,
        bool needToUpdate, DateTimeOffset lastUpdate) : base(name, locationComponents, geometry, osm, needToUpdate,
        lastUpdate)
    {
        Id = id;
    }

    [JsonPropertyName("id")] public int Id { get; }
}