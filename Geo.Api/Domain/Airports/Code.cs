namespace Geo.Api.Domain;

internal sealed record Code(string En)
{
    public string? Ru { get; init; }
}
