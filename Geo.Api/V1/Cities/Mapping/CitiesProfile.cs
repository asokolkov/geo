using AutoMapper;
using Geo.Api.Repositories.Cities.Models;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.V1.Cities.Models;
using Geo.Api.V1.Countries.Models;

namespace Geo.Api.V1.Cities.Mapping;

internal sealed class CitiesProfile : Profile
{
    public CitiesProfile()
    {
        CreateMap<CityEntity, CityDtoWithId>().ConstructUsing(entity => new CityDtoWithId(
            entity.Id,
            new CityCodeDto(entity.Code),
            new CityNameDto(entity.Name.Ru) { En = entity.Name.En },
            new CityLocationComponentsDto(entity.CountryId) { RegionId = entity.RegionId },
            new CityGeometryDto { Lat = entity.Geometry.Lat, Lon = entity.Geometry.Lon },
            entity.Osm, entity.NeedToUpdate, entity.UpdatedAt
        )
        {
             UtcOffset = entity.UtcOffset
        });
    }
}