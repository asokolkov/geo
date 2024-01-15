using Geo.Api.Repositories.Exceptions;
using Geo.Api.Repositories.Regions.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Geo.Api.Repositories.Regions;

internal sealed class RegionsRepository : IRegionsRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;

    public RegionsRepository(ILogger<RegionsRepository> logger, ApplicationContext context,
        ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<RegionEntity?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var regionEntity = await context.Regions
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return regionEntity;
    }

    public async Task<RegionEntity> CreateAsync(int countryId, RegionNameEntity name, string osm,
        bool needToUpdate, RegionGeometryEntity? geometry = default, int? utcOffset = default,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var newRegion = new RegionEntity(0, countryId, name, osm, needToUpdate, now)
        {
            Geometry = geometry ?? new RegionGeometryEntity(),
            UtcOffset = utcOffset
        };
        var entry = await context.AddAsync(newRegion, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await entry.ReloadAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<RegionEntity> UpdateAsync(int id, int countryId, RegionNameEntity name, string osm,
        bool needToUpdate, RegionGeometryEntity? geometry = default, int? utcOffset = default,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var regionEntity = await context.Regions
            .FirstOrDefaultAsync(region => region.Id == id, cancellationToken);

        if (regionEntity is null)
        {
            logger.LogWarning("Region with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(RegionEntity), id);
        }

        regionEntity.Name = name;
        regionEntity.CountryId = countryId;
        regionEntity.Osm = osm;
        regionEntity.UpdatedAt = now;
        regionEntity.NeedToUpdate = needToUpdate;
        regionEntity.Geometry = geometry ?? new RegionGeometryEntity();
        regionEntity.UtcOffset = utcOffset;

        await context.SaveChangesAsync(cancellationToken);
        return regionEntity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var regionEntity = await context.Regions.FirstOrDefaultAsync(region => region.Id == id, cancellationToken);
        if (regionEntity is null)
        {
            logger.LogWarning("Region with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(RegionEntity), id);
        }

        regionEntity.UpdatedAt = now;
        regionEntity.DeletedAt = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}