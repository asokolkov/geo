using Geo.Api.Repositories.Exceptions;
using AutoMapper;
using Geo.Api.Domain.Regions;
using Geo.Api.Repositories.Regions.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Geo.Api.Repositories.Regions;

internal sealed class RegionsRepository : IRegionsRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly IMapper mapper;
    private readonly ISystemClock systemClock;

    public RegionsRepository(ILogger<RegionsRepository> logger, ApplicationContext context, IMapper mapper,
        ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.mapper = mapper;
        this.systemClock = systemClock;
    }

    public async Task<Region?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var regionEntity = await context.Regions
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.Country)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return mapper.Map<Region>(regionEntity);
    }

    public async Task<Region> CreateAsync(int countryId, string name, string osm,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var newRegion = new RegionEntity(0, countryId, name, osm, now);
        var entry = await context.AddAsync(newRegion, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Region>(entry.Entity);
    }

    public async Task<Region> UpdateAsync(int id, int countryId, string name, string osm,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var regionEntity = await context.Regions
            .Include(e => e.Country)
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (regionEntity is null)
        {
            logger.LogWarning("Region with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(Region), id);
        }

        regionEntity.Name = name;
        regionEntity.CountryId = countryId;
        regionEntity.Osm = osm;
        regionEntity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Region>(regionEntity);
    }
}