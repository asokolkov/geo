using Geo.Api.Repositories.Airports.Models;
using Geo.Api.Repositories.Airports.TypeConfiguration;
using Geo.Api.Repositories.Cities.Models;
using Geo.Api.Repositories.Cities.TypeConfigurations;
using Geo.Api.Repositories.Countries.TypeConfigurations;
using Geo.Api.Repositories.RailwayStations.Models;
using Geo.Api.Repositories.RailwayStations.TypeConfigurations;
using Geo.Api.Repositories.Regions.TypeConfigurations;
using Geo.Api.Repositories.SubwayStations.Models;
using Geo.Api.Repositories.SubwayStations.TypeConfiguration;

namespace Geo.Api.Repositories;

using Countries.Models;
using Microsoft.EntityFrameworkCore;
using Regions.Models;

internal sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<CountryEntity> Countries { get; private set; } = null!;
    
    public DbSet<RegionEntity> Regions { get; private set; } = null!;

    public DbSet<CityEntity> Cities { get; private set; } = null!;

    public DbSet<AirportEntity> Airports { get; private set; } = null!;
    
    public DbSet<RailwayStationEntity> RailwayStations { get; private set; } = null!;

    public DbSet<SubwayStationEntity> SubwayStations { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AirportEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CityEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CountryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayStationEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RegionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SubwayStationEntityTypeConfiguration());
    }
}