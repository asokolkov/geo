namespace Geo.Api.Repositories.Regions.Models;

using Countries.Models;

public sealed class RegionEntity
{
    public RegionEntity(int id, int countryId, RegionNameEntity name, string osm, bool needToUpdate, DateTimeOffset updatedAt)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        Osm = osm;
        UpdatedAt = updatedAt;
        NeedToUpdate = needToUpdate;
    }
    
#pragma warning disable CS8618
    private RegionEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public int CountryId { get; set; }

    public RegionNameEntity Name { get; set; }

    public string Osm { get; set; }
    
    public bool NeedToUpdate { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public RegionGeometryEntity Geometry { get; set; } = new();
    
    public int? UtcOffset { get; set; }
}