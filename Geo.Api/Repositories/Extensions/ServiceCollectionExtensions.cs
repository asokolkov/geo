using Geo.Api.Repositories.Airports;
using Geo.Api.Repositories.Airports.Models;
using Geo.Api.Repositories.Airports.TypeConfiguration;
using Geo.Api.Repositories.Cities;
using Geo.Api.Repositories.Cities.Models;
using Geo.Api.Repositories.Cities.TypeConfigurations;
using Geo.Api.Repositories.Countries;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.Repositories.Countries.TypeConfigurations;
using Geo.Api.Repositories.RailwayStations;
using Geo.Api.Repositories.RailwayStations.Models;
using Geo.Api.Repositories.RailwayStations.TypeConfigurations;
using Geo.Api.Repositories.Regions;
using Geo.Api.Repositories.Regions.Models;
using Geo.Api.Repositories.Regions.TypeConfigurations;
using Geo.Api.Repositories.SubwayStations;
using Geo.Api.Repositories.SubwayStations.Models;
using Geo.Api.Repositories.SubwayStations.TypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Geo.Api.Repositories.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(config =>
        {
            var connectionString = configuration.GetConnectionString("GeoDb");
            config.UseNpgsql(connectionString);
        });
        services.AddScoped<IAirportsRepository, AirportsRepository>();
        services.AddScoped<ICitiesRepository, CitiesRepository>();
        services.AddScoped<ICountriesRepository, CountriesRepository>();
        services.AddScoped<IRailwayStationsRepository, RailwayStationsRepository>();
        services.AddScoped<IRegionsRepository, RegionsRepository>();
        services.AddScoped<ISubwayStationsRepository, SubwayStationsRepository>();

        services.AddSingleton<IEntityTypeConfiguration<AirportEntity>, AirportEntityTypeConfiguration>();
        services.AddSingleton<IEntityTypeConfiguration<CityEntity>, CityEntityTypeConfiguration>();
        services.AddSingleton<IEntityTypeConfiguration<CountryEntity>, CountryEntityTypeConfiguration>();
        services.AddSingleton<IEntityTypeConfiguration<RailwayStationEntity>, RailwayStationEntityTypeConfiguration>();
        services.AddSingleton<IEntityTypeConfiguration<RegionEntity>, RegionEntityTypeConfiguration>();
        services.AddSingleton<IEntityTypeConfiguration<SubwayStationEntity>, SubwayStationEntityTypeConfiguration>();

        return services;
    }
}