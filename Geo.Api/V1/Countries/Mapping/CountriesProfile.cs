using AutoMapper;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.V1.Countries.Models;

namespace Geo.Api.V1.Countries.Mapping;

internal sealed class CountriesProfile : Profile
{
    public CountriesProfile()
    {
        CreateMap<CountryEntity, CountryDtoWithId>().ConstructUsing(entity => new CountryDtoWithId(
            entity.Id,
            new CountryCodeDto(entity.Iso3116Alpha2Code, entity.Iso3166Alpha3Code),
            new CountryNameDto(entity.Name.Ru) { En = entity.Name.En },
            new CountryPhoneDto(entity.PhoneCode, entity.PhoneMask),
            new CountryGeometryDto { Lat = entity.Geometry.Lat, Lon = entity.Geometry.Lon },
            entity.Osm,
            entity.NeedAutomaticUpdate,
            entity.UpdatedAt
        ));
    }
}