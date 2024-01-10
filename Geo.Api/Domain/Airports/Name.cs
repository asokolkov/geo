namespace Geo.Api.Domain.Airports;

internal sealed record Name(string Ru)
{
    public string? En { get; init; } 
}