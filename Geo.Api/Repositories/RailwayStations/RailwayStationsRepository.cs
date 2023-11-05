using AutoMapper;
using Geo.Api.Domain.RailwayStation;
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
    private readonly IMapper mapper;

    public RailwayStationsRepository(ILogger<RailwayStationsRepository> logger, ApplicationContext context, ISystemClock systemClock, IMapper mapper)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
        this.mapper = mapper;
    }

    public async Task<RailwayStation?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.RailwayStations
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.City)
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
        return mapper.Map<RailwayStation>(entity);
    }

    public async Task<RailwayStation> CreateAsync(int cityId, int rzdCode, bool isMain, string name, double latitude, double longitude, string timezone,
        string osm, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity =
            new RailwayStationEntity(0, cityId, rzdCode, isMain, name, latitude, longitude, timezone, osm, now);
        var entry = await context.AddAsync(countryEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<RailwayStation>(entry.Entity);
    }

    public async Task<RailwayStation> UpdateAsync(int id, int cityId, int rzdCode, bool isMain, string name, double latitude, double longitude,
        string timezone, string osm, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var entity = await context.RailwayStations.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (entity is null)
        {
            logger.LogWarning("RailwayStation with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(RailwayStation), id);
        }

        entity.CityId = cityId;
        entity.RzdCode = rzdCode;
        entity.IsMain = isMain;
        entity.Name = name;
        entity.Latitude = latitude;
        entity.Longitude = longitude;
        entity.Timezone = timezone;
        entity.Osm = osm;
        entity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<RailwayStation>(entity);
    }
}