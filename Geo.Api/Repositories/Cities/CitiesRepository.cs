using AutoMapper;
using Geo.Api.Domain.Cities;
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
    private readonly IMapper mapper;

    public CitiesRepository(ILogger<CitiesRepository> logger, ApplicationContext context, ISystemClock systemClock, IMapper mapper)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
        this.mapper = mapper;
    }

    public async Task<City?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Cities
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.Country)
            .Include(e => e.Region)
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
        return mapper.Map<City>(entity);
    }

    public async Task<City> CreateAsync(int countryId, int? regionId, string name, double latitude, double longitude,
        string timezone, string osm, string? iata, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var cityEntity =
            new CityEntity(0, countryId, name, latitude, longitude, timezone, osm, now)
            {
                Iata = iata,
                RegionId = regionId
            };

        var entry = await context.AddAsync(cityEntity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<City>(entry.Entity);
    }

    public async Task<City> UpdateAsync(int id, int countryId, int? regionId, string name, double latitude, double longitude,
        string timezone, string osm, string? iata, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var cityEntity = await context.Cities.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (cityEntity is null)
        {
            logger.LogWarning("City with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(City), id);
        }

        cityEntity.CountryId = countryId;
        cityEntity.Name = name;
        cityEntity.RegionId = regionId;
        cityEntity.Latitude = latitude;
        cityEntity.Longitude = longitude;
        cityEntity.Timezone = timezone;
        cityEntity.Osm = osm;
        cityEntity.Iata = iata;
        cityEntity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<City>(cityEntity);
    }
}