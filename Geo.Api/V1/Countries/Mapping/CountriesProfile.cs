using AutoMapper;
using Geo.Api.Domain.Countries;
using Geo.Api.V1.Countries.Models;

namespace Geo.Api.V1.Countries.Mapping;

internal sealed class CountriesProfile : Profile
{
    public CountriesProfile()
    {
        CreateMap<V1CountryDto, Country>();
    }
}