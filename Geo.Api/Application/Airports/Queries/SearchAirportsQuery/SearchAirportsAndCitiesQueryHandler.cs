using Geo.Api.Domain;
using Geo.Api.Repositories.Airports;
using Geo.Api.Repositories.Airports.Models;
using Geo.Api.Repositories.Cities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Geo.Api.Application.Airports.Queries.SearchAirportsQuery;

internal sealed class SearchAirportsAndCitiesQueryHandler : IRequestHandler<SearchAirportsAndCitiesQuery, SearchResult>
{
    private readonly ILogger logger;
    private readonly IAirportsRepository airportsRepository;
    private readonly ICitiesRepository citiesRepository;

    public SearchAirportsAndCitiesQueryHandler(ILogger<SearchAirportsAndCitiesQueryHandler> logger,
        IAirportsRepository airportsRepository, ICitiesRepository citiesRepository)
    {
        this.logger = logger;
        this.airportsRepository = airportsRepository;
        this.citiesRepository = citiesRepository;
    }

    public async Task<SearchResult> Handle(SearchAirportsAndCitiesQuery request, CancellationToken cancellationToken)
    {
        using var termScope = logger.BeginScope("[Term='{Term}']", request.Term);

        logger.LogInformation("Getting airports");
        var airports = await airportsRepository.Queryable
            .Include(e => e.Translations)
            .Where(entity => FilterAirports(entity, request.Term))
            .ToListAsync(cancellationToken);
        
        var cities = await citiesRepository.Queryable
            .Include(e => e.Translations)
            .Where(city =FilterAirports())
    }

    private bool FilterAirports(AirportEntity entity, string term)
    {
        return entity.IataEn.Contains(term, StringComparison.InvariantCultureIgnoreCase)
               || (entity.IataRu != null && entity.IataRu.Contains(term, StringComparison.InvariantCultureIgnoreCase))
               || entity.Translations.Any(translation => translation.Translation.Contains(term));
    }
}