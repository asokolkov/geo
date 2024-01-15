using Geo.Api.Repositories.Exceptions;
using Geo.Api.Repositories.RailwayStations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Geo.Api.Repositories.RailwayStations;

internal sealed class RailwayStationsRepository : IRailwayStationsRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;

    public RailwayStationsRepository(ILogger<RailwayStationsRepository> logger, ApplicationContext context,
        ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<RailwayStationEntity?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.RailwayStations
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.City)
            .FirstOrDefaultAsync(country => country.Id == id || country.Code.Express3 == id, cancellationToken);
        return entity;
    }

    public async Task<RailwayStationEntity> CreateAsync(int cityId, RailwayStationCodeEntity code,
        RailwayStationNameEntity name, RailwayStationGeometryEntity geometry,
        string osm, bool needToUpdate, int? utcOffset = default, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity =
            new RailwayStationEntity(0, cityId, code, name, geometry, osm, needToUpdate, now) { UtcOffset = utcOffset };
        var entry = await context.AddAsync(countryEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await entry.Reference(e => e.City).LoadAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<RailwayStationEntity> UpdateAsync(int id, int cityId, RailwayStationCodeEntity code,
        RailwayStationNameEntity name, RailwayStationGeometryEntity geometry,
        string osm, bool needToUpdate, int? utcOffset = default, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var entity = await context.RailwayStations.FirstOrDefaultAsync(
            railwayStationEntity => railwayStationEntity.Id == id || railwayStationEntity.Code.Express3 == id,
            cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("RailwayStation with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(RailwayStationEntity), id);
        }

        entity.CityId = cityId;
        entity.Code = code;
        entity.Name = name;
        entity.Geometry = geometry;
        entity.UtcOffset = utcOffset;
        entity.Osm = osm;
        entity.UpdatedAt = now;
        entity.NeedToUpdate = needToUpdate;

        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(entity).Reference(e => e.City).LoadAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity =
            await context.RailwayStations.FirstOrDefaultAsync(e => e.Id == id || e.Code.Express3 == id,
                cancellationToken);
        if (countryEntity is null)
        {
            logger.LogWarning("RailwayStation with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(RailwayStationEntity), id);
        }

        countryEntity.DeletedAt = now;
        countryEntity.UpdatedAt = now;
        await context.SaveChangesAsync(cancellationToken);
    }
}