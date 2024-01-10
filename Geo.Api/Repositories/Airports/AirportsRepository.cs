using Geo.Api.Repositories.Airports.Models;
using Geo.Api.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Geo.Api.Repositories.Airports;

internal sealed class AirportsRepository : IAirportsRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;


    public AirportsRepository(ILogger<AirportsRepository> logger, ApplicationContext context, ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<AirportEntity?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Airports
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.City)
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
    }

    public async Task<AirportEntity?> GetAsync(string iata, CancellationToken cancellationToken = default)
    {
        return await context.Airports
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.City)
            .FirstOrDefaultAsync(country => country.IataEn == iata || country.IataRu == iata, cancellationToken);
    }

    public async Task<AirportEntity> CreateAsync(int cityId, string name, string iataEn, string? iataRu,
        double latitude, double longitude, int utcOffset, string osm, bool needAutomaticUpdate = true,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity =
            new AirportEntity(0, cityId, name, iataEn, latitude, longitude, utcOffset, osm, now)
            {
                IataRu = iataRu,
                NeedAutomaticUpdate = needAutomaticUpdate
            };

        var entry = await context.AddAsync(airportEntity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<AirportEntity> UpdateAsync(int id, int cityId, string name, string iataEn, string? iataRu,
        double latitude, double longitude, int utcOffset, string osm, bool needAutomaticUpdate = true,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity = await context.Airports.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (airportEntity is null)
        {
            logger.LogWarning("Airport with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(AirportEntity), id);
        }

        airportEntity.CityId = cityId;
        airportEntity.Name = name;
        airportEntity.IataEn = iataEn;
        airportEntity.IataRu = iataRu;
        airportEntity.Latitude = latitude;
        airportEntity.Longitude = longitude;
        airportEntity.UtcOffset = utcOffset;
        airportEntity.Osm = osm;
        airportEntity.UpdatedAt = now;
        airportEntity.NeedAutomaticUpdate = needAutomaticUpdate;

        await context.SaveChangesAsync(cancellationToken);
        return airportEntity;
    }

    public IQueryable<AirportEntity> Queryable => context.Airports.AsQueryable().AsNoTracking();
}