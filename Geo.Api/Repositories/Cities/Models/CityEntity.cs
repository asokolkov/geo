using Geo.Api.Repositories.Countries.Models;
using Geo.Api.Repositories.Regions.Models;
using Geo.Api.Repositories.Translations.Models;

namespace Geo.Api.Repositories.Cities.Models;

internal sealed class CityEntity
{
    public CityEntity(int id, int countryId, string name,  double latitude, double longitude, string timezone, string osm,
        DateTimeOffset updatedAt)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
        Osm = osm;
        UpdatedAt = updatedAt;
    }
    
#pragma warning disable CS8618
    private CityEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }
    
    public int CountryId { get; set; }

    public string Name { get; set; }
    
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Timezone { get; set; }

    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
    
    public int? RegionId { get; set; }
    
    public string? Iata { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public CountryEntity? Country { get; set; }

    public RegionEntity? Region { get; set; }
    
    public List<TranslationEntity>? Translations { get; init; }
}