using AutoMapper;
using Geo.Api.Repositories.Airports.Models;
using Geo.Api.V1.Airports.Models;

namespace Geo.Api.V1.Airports.Mapping;

internal sealed class AirportsProfile : Profile
{
    public AirportsProfile()
    {
        CreateMap<AirportEntity, AirportDtoWithId>().ConstructUsing(entity => new AirportDtoWithId(
            entity.Id,
            new AirportCodeDto(entity.Code.En) {Ru = entity.Code.Ru},
            new AirportNameDto(entity.Name.Ru) {En = entity.Name.En},
            new AirportLocationComponentsDto(entity.City!.Id, entity.City.CountryId) {RegionId = entity.City.RegionId},
            new AirportGeometryDto{Lat = entity.Geometry.Lat, Lon = entity.Geometry.Lon},
            entity.Osm, entity.NeedAutomaticUpdate, entity.UpdatedAt
        )
        {
            UtcOffset = entity.UtcOffset
        });
    }
}