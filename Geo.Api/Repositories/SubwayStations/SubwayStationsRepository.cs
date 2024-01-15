using Geo.Api.Repositories.Exceptions;
using Geo.Api.Repositories.SubwayStations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Geo.Api.Repositories.SubwayStations;

internal sealed class SubwayStationsRepository : ISubwayStationsRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;


    public SubwayStationsRepository(ILogger<SubwayStationsRepository> logger, ApplicationContext context, ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<SubwayStationEntity?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.SubwayStations
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.City)
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
    }

    public async Task<SubwayStationEntity> CreateAsync(int cityId, SubwayStationNameEntity name, SubwayLineNameEntity lineName,
        SubwayStationGeometryEntity geometry, string osm, bool needAutomaticUpdate,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity =
            new SubwayStationEntity(0, name, lineName, geometry, cityId, osm, needAutomaticUpdate, now);

        var entry = await context.AddAsync(airportEntity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        await entry.Reference(e => e.City).LoadAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<SubwayStationEntity> UpdateAsync(int id, int cityId, SubwayStationNameEntity name, SubwayLineNameEntity lineName,
        SubwayStationGeometryEntity geometry, string osm, bool needAutomaticUpdate,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var subwayStationEntity =
            await context.SubwayStations.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (subwayStationEntity is null)
        {
            logger.LogWarning("SubwayStation with id '{Id}' not found", cityId);
            throw new EntityNotFoundException(typeof(SubwayStationEntity), cityId);
        }

        subwayStationEntity.CityId = cityId;
        subwayStationEntity.StationName = name;
        subwayStationEntity.LineName = lineName;
        subwayStationEntity.Geometry = geometry;
        subwayStationEntity.Osm = osm;
        subwayStationEntity.UpdatedAt = now;
        subwayStationEntity.NeedToUpdate = needAutomaticUpdate;

        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(subwayStationEntity).Reference(e => e.City).LoadAsync(cancellationToken);
        return subwayStationEntity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var subwayStationEntoty = await context.SubwayStations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (subwayStationEntoty is null)
        {
            logger.LogWarning("SubwayStation with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(SubwayStationEntity), id);
        }

        subwayStationEntoty.DeletedAt = now;
        subwayStationEntoty.UpdatedAt = now;
        await context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<SubwayStationEntity> Queryable => context.SubwayStations.AsQueryable().AsNoTracking().Include(e => e.City);
}