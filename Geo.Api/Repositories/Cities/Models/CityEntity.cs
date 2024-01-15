namespace Geo.Api.Repositories.Cities.Models;

public sealed class CityEntity
{
    public CityEntity(int id, string code, int countryId, CityNameEntity name, CityGeometryEntity geometry, string osm,
        bool needToUpdate, DateTimeOffset updatedAt)
    {
        Id = id;
        Code = code;
        CountryId = countryId;
        Name = name;
        Geometry = geometry;
        Osm = osm;
        UpdatedAt = updatedAt;
        NeedToUpdate = needToUpdate;
    }
    
#pragma warning disable CS8618
    private CityEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }
    
    public string Code { get; set; }
    
    public int CountryId { get; set; }

    public CityNameEntity Name { get; set; }
    
    public CityGeometryEntity Geometry { get; set; }
    
    public string Osm { get; set; }
    
    public bool NeedToUpdate { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
    
    public int? RegionId { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    
    public int? UtcOffset { get; set; }
}