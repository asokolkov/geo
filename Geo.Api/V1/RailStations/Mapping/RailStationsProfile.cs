using AutoMapper;
using Geo.Api.Repositories.Airports.Models;
using Geo.Api.Repositories.RailwayStations.Models;
using Geo.Api.V1.Airports.Models;
using Geo.Api.V1.RailStations.Models;

namespace Geo.Api.V1.RailStations.Mapping;

internal sealed class RailStationsProfile : Profile
{
    public RailStationsProfile()
    {
        CreateMap<RailwayStationEntity, RailStationDtoWithId>().ConstructUsing(entity => new RailStationDtoWithId(
            entity.Id,
            new RailStationCodeDto(entity.Code.Express3),
            new RailStationNameDto(entity.Name.En) { Ru = entity.Name.Ru },
            new RailStationLocationComponentsDto(entity.City!.Id, entity.City.CountryId)
                { RegionId = entity.City.RegionId },
            new RailStationGeometryDto{Lat = entity.Geometry.Lat, Lon = entity.Geometry.Lon},
            entity.Osm, entity.NeedToUpdate, entity.UpdatedAt
        )
        {
            
            UtcOffset = entity.UtcOffset
        });
    }
}