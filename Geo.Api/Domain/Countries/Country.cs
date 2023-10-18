namespace Geo.Api.Domain.Countries;

internal sealed class Country
{
    public Country(int id, string name, string iso3116Alpha2Code, string iso3166Alpha3Code, string phoneCode,
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

    public int Id { get; }

    public string Name { get; }

    public string Iso3116Alpha2Code { get; }

    public string Iso3166Alpha3Code { get; }

    public string PhoneCode { get; }

    public string PhoneMask { get; }
    
    public string Osm { get; }

    public DateTimeOffset UpdatedAt { get; }

    public DateTimeOffset? DeletedAt { get; init; }
}