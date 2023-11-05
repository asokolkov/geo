namespace Geo.Api.Repositories.Countries.Models;

internal sealed class CountryEntity
{
    public CountryEntity(int id, string name, string iso3116Alpha2Code, string iso3166Alpha3Code, string phoneCode,
        string phoneMask, string osm, DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        Iso3116Alpha2Code = iso3116Alpha2Code;
        Iso3166Alpha3Code = iso3166Alpha3Code;
        PhoneCode = phoneCode;
        PhoneMask = phoneMask;
        Osm = osm;
        UpdatedAt = updatedAt;
    }
    
#pragma warning disable CS8618
    private CountryEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public string Name { get; set; }

    public string Iso3116Alpha2Code { get; set; }

    public string Iso3166Alpha3Code { get; set; }

    public string PhoneCode { get; set; }

    public string PhoneMask { get; set; }
    
    public string Osm { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
}