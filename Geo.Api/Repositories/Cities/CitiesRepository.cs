using Geo.Api.Repositories.Cities.Models;
using Geo.Api.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Geo.Api.Repositories.Cities;

internal sealed class CitiesRepository : ICitiesRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;

    public CitiesRepository(ILogger<CitiesRepository> logger, ApplicationContext context, ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<CityEntity?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Cities
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
        return entity;
    }
    
    public async Task<CityEntity?> GetAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await context.Cities
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(country => country.Code == code, cancellationToken);
        return entity;
    }

    public async Task<CityEntity> CreateAsync(int countryId, int? regionId, CityNameEntity name, CityGeometryEntity geometry,
        string osm, string code, bool needToUpdate, int? utcOffset, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var cityEntity =
            new CityEntity(0, code, countryId, name, geometry, osm, needToUpdate, now)
            {
                RegionId = regionId,
                UtcOffset = utcOffset
            };

        var entry = await context.AddAsync(cityEntity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        await entry.ReloadAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<CityEntity> UpdateAsync(int id, int countryId, int? regionId, CityNameEntity name, CityGeometryEntity geometry,
        string osm, string code, bool needToUpdate, int? utcOffset, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var cityEntity = await context.Cities.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (cityEntity is null)
        {
            logger.LogWarning("City with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(CityEntity), id);
        }

        cityEntity.CountryId = countryId;
        cityEntity.Name = name;
        cityEntity.RegionId = regionId;
        cityEntity.Geometry = geometry;
        cityEntity.Osm = osm;
        cityEntity.UpdatedAt = now;
        cityEntity.Code = code;
        cityEntity.NeedToUpdate = needToUpdate;
        cityEntity.UtcOffset = utcOffset;

        await context.SaveChangesAsync(cancellationToken);
        return cityEntity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity = await context.Countries.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (countryEntity is null)
        {
            logger.LogWarning("Country with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(CityEntity), id);
        }

        countryEntity.DeletedAt = now;
        countryEntity.UpdatedAt = now;
        await context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<CityEntity> Queryable => context.Cities.AsQueryable().AsNoTracking();
}