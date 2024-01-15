using Geo.Api.V1.Airports.Mapping;
using Geo.Api.V1.Cities.Mapping;
using Geo.Api.V1.Countries.Mapping;
using Geo.Api.V1.RailStations.Mapping;
using Geo.Api.V1.Regions.Mapping;
using Geo.Api.V1.SubwayStations.Mapping;

namespace Geo.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CountriesProfile>();
            cfg.AddProfile<AirportsProfile>();
            cfg.AddProfile<CitiesProfile>();
            cfg.AddProfile<RailStationsProfile>();
            cfg.AddProfile<RegionsProfile>();
            cfg.AddProfile<SubwayStationsProfile>();
        });
        return services;
    }
}