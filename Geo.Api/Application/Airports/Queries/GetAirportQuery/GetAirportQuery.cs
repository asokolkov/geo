using Geo.Api.Repositories.Airports.Models;
using MediatR;

namespace Geo.Api.Application.Airports.Queries.GetAirportByIdQuery;

internal sealed record GetAirportQuery : IRequest<IReadOnlyCollection<AirportEntity>>
{
    public int? Id { get; init; }
    
    public string? Code { get; init; }
}