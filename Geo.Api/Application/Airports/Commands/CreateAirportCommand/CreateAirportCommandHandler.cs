using Geo.Api.Domain.Airports;
using Geo.Api.Repositories.Airports;
using Geo.Api.Repositories.Translations;
using MediatR;

namespace Geo.Api.Application.Airports.Commands.CreateAirportCommand;

internal sealed class CreateAirportCommandHandler : IRequestHandler<CreateAirportCommand, Airport>
{
    private readonly ILogger logger;
    private readonly IAirportsRepository airportsRepository;
    private readonly ITranslationsRepository translationsRepository;

    public CreateAirportCommandHandler(ILogger<CreateAirportCommandHandler> logger,
        IAirportsRepository airportsRepository,
        ITranslationsRepository translationsRepository)
    {
        this.logger = logger;
        this.airportsRepository = airportsRepository;
        this.translationsRepository = translationsRepository;
    }

    public async Task<Airport> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating airport '{AirportName}'", request.Name);
        var airportEntity = await airportsRepository.CreateAsync(
            request.LocationComponents.CityId,
            request.Name.Ru,
            request.Code.En,
            request.Code.Ru,
            request.Geometry.Latitude,
            request.Geometry.Longitude,
            request.UtcOffset,
            request.Osm,
            cancellationToken);

        if (request.Name.En is not null)
            await translationsRepository.CreateAsync(airportEntity.Id, "airport", "en", request.Name.En,
                cancellationToken);

        return new Airport(airportEntity.Id, request.Code, request.Name, request.Geometry, request.LocationComponents,
            airportEntity.UtcOffset, airportEntity.Osm);
    }
}