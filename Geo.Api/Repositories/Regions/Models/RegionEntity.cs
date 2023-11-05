namespace Geo.Api.Repositories.Regions.Models;

using Countries.Models;

internal sealed class RegionEntity
{
    public RegionEntity(int id, int countryId, string name, string osm, DateTimeOffset updatedAt)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        Osm = osm;
        UpdatedAt = updatedAt;
    }
    
#pragma warning disable CS8618
    private RegionEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public int CountryId { get; set; }

    public string Name { get; set; }

    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    
    public CountryEntity? Country { get; set; }
}