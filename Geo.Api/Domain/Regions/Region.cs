namespace Geo.Api.Domain.Regions;

using Countries;

internal sealed class Region
{
    public Region(int id, int countryId, string name, string osm, DateTimeOffset updatedAt)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        Osm = osm;
        UpdatedAt = updatedAt;
    }

    public int Id { get; }
    
    public int CountryId { get; }

    public string Name { get; }

    public string Osm { get; }

    public DateTimeOffset UpdatedAt { get; }

    public DateTimeOffset? DeletedAt { get; init; }
    
    public Country? Country { get; init; }
}