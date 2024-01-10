using Geo.Api.Repositories.Airports;
using Geo.Api.Repositories.Airports.Models;
using MediatR;

namespace Geo.Api.Application.Airports.Queries.GetAirportByIdQuery;

internal sealed class GetAirportQueryHandler : IRequestHandler<GetAirportQuery, IReadOnlyCollection<AirportEntity>>
{
    private readonly ILogger logger;
    private readonly IAirportsRepository repository;

    public GetAirportQueryHandler(ILogger<GetAirportQueryHandler> logger, IAirportsRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<IReadOnlyCollection<AirportEntity>> Handle(GetAirportQuery request,
        CancellationToken cancellationToken)
    {
        var airports = new List<AirportEntity>();
        if (request.Id is not null)
        {
            logger.LogInformation("Getting airport by id '{AirportId}'", request.Id);
            var airport = await repository.GetAsync(request.Id.Value, cancellationToken);
            if (airport is null)
                logger.LogWarning("Airport with id '{AirportId}' not found", request.Id);
            else
                airports.Add(airport);
        }

        if (request.Code is not null)
        {
            logger.LogInformation("Getting airport by code '{AirportCode}'", request.Code);
            var airport = await repository.GetAsync(request.Code, cancellationToken);
            if (airport is null)
                logger.LogWarning("Airport with code '{AirportCode}' not found", request.Code);
            else
                airports.Add(airport);
        }

        return airports;
    }
}