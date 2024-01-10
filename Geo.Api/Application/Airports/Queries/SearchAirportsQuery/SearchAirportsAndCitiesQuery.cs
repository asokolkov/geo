using Geo.Api.Domain;
using MediatR;

namespace Geo.Api.Application.Airports.Queries.SearchAirportsQuery;

internal sealed record SearchAirportsAndCitiesQuery(string Term) : IRequest<SearchResult>;
