using AutoMapper;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.Repositories.Regions.Models;
using Geo.Api.V1.Countries.Models;
using Geo.Api.V1.Regions.Models;

namespace Geo.Api.V1.Regions.Mapping;

internal sealed class RegionsProfile : Profile
{
    public RegionsProfile()
    {
        CreateMap<RegionEntity, RegionDtoWithId>().ConstructUsing(entity => new RegionDtoWithId(
            entity.Id,
            new RegionNameDto(entity.Name.Ru) { En = entity.Name.En },
            new RegionLocationComponentsDto(entity.CountryId),
            new RegionGeometryDto { Lat = entity.Geometry.Lat, Lon = entity.Geometry.Lon },
            entity.Osm,
            entity.NeedToUpdate,
            entity.UpdatedAt
        )
        {
            UtcOffset = entity.UtcOffset,
        });
    }
}