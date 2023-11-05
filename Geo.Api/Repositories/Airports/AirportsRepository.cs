using AutoMapper;
using Geo.Api.Domain.Airports;
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
    private readonly IMapper mapper;

    public AirportsRepository(ILogger<AirportsRepository> logger, ApplicationContext context, ISystemClock systemClock,
        IMapper mapper)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
        this.mapper = mapper;
    }

    public async Task<Airport?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Airports
            .AsQueryable()
            .AsNoTracking()
            .Include(e => e.City)
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
        return mapper.Map<Airport>(entity);
    }

    public async Task<Airport> CreateAsync(int cityId, string name, string iataEn, string? iataRu, double latitude,
        double longitude, string timezone, string osm, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity =
            new AirportEntity(0, cityId, name, iataEn, latitude, longitude, timezone, osm, now)
            {
                IataRu = iataRu
            };

        var entry = await context.AddAsync(airportEntity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Airport>(entry.Entity);
    }

    public async Task<Airport> UpdateAsync(int id, int cityId, string name, string iataEn, string? iataRu,
        double latitude, double longitude, string timezone, string osm, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var airportEntity = await context.Airports.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (airportEntity is null)
        {
            logger.LogWarning("Airport with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(Airport), id);
        }

        airportEntity.CityId = cityId;
        airportEntity.Name = name;
        airportEntity.IataEn = name;
        airportEntity.IataRu = iataRu;
        airportEntity.Latitude = latitude;
        airportEntity.Longitude = longitude;
        airportEntity.Timezone = timezone;
        airportEntity.Osm = osm;
        airportEntity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Airport>(airportEntity);
    }
}