using Geo.Api.Domain;
using Geo.Api.Domain.Airports;
using MediatR;

namespace Geo.Api.Application.Airports.Commands.CreateAirportCommand;

internal sealed record CreateAirportCommand(
    Code Code,
    Name Name,
    AirportLocationComponents LocationComponents,
    Geometry Geometry,
    int UtcOffset,
    string Osm
) : IRequest<Airport>;