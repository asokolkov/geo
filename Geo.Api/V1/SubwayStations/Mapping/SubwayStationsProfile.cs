using AutoMapper;
using Geo.Api.Repositories.SubwayStations.Models;
using Geo.Api.V1.SubwayStations.Models;

namespace Geo.Api.V1.SubwayStations.Mapping;

internal sealed class SubwayStationsProfile : Profile
{
    public SubwayStationsProfile()
    {
        CreateMap<SubwayStationEntity, SubwayStationDtoWithId>().ConstructUsing(entity => new SubwayStationDtoWithId(
            entity.Id,
            new SubwayStationNameDto(entity.StationName.En) {Ru = entity.StationName.Ru},
            new SubwayLineNameDto(entity.LineName.En) {Ru = entity.LineName.Ru},
            new SubwayLocationComponentsDto(entity.City!.Id, entity.City.CountryId) {RegionId = entity.City.RegionId},
            new SubwayGeometryDto{Lat = entity.Geometry.Lat, Lon = entity.Geometry.Lon},
            entity.Osm, entity.NeedToUpdate, entity.UpdatedAt
        )
        {
        });
    }
}