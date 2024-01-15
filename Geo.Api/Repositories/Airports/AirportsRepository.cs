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
            .FirstOrDefaultAsync(country => country.Code.En == iata || country.Code.Ru == iata, cancellationToken);
    }

    public async Task<AirportEntity> CreateAsync(int cityId, AirportNameEntity name, AirportCodeEntity code,
        AirportGeometryEntity geometry, string osm, bool needAutomaticUpdate, int? utcOffset = default,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity =
            new AirportEntity(0, cityId, name, code, geometry, osm, needAutomaticUpdate, now)
            {
                UtcOffset = utcOffset
            };

        var entry = await context.AddAsync(airportEntity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        await entry.Reference(e => e.City).LoadAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<AirportEntity> UpdateAsync(int id, int cityId, AirportNameEntity name, AirportCodeEntity code,
        AirportGeometryEntity geometry, string osm, bool needAutomaticUpdate, int? utcOffset = default,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity =
            await context.Airports.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (airportEntity is null)
        {
            logger.LogWarning("Airport with id '{Id}' not found", cityId);
            throw new EntityNotFoundException(typeof(AirportEntity), cityId);
        }

        airportEntity.CityId = cityId;
        airportEntity.Name = name;
        airportEntity.Code = code;
        airportEntity.Geometry = geometry;
        airportEntity.UtcOffset = utcOffset;
        airportEntity.Osm = osm;
        airportEntity.UpdatedAt = now;
        airportEntity.NeedAutomaticUpdate = needAutomaticUpdate;

        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(airportEntity).Reference(e => e.City).LoadAsync(cancellationToken);
        return airportEntity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity = await context.Countries.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (countryEntity is null)
        {
            logger.LogWarning("Country with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(AirportEntity), id);
        }

        countryEntity.DeletedAt = now;
        countryEntity.UpdatedAt = now;
        await context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<AirportEntity> Queryable => context.Airports.AsQueryable().AsNoTracking().Include(e => e.City);
}