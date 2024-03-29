﻿namespace Geo.Api.Repositories.Countries.Models;

public sealed class CountryEntity
{
    public CountryEntity(int id, CountryNameEntity name, CountryGeometryEntity geometry, string iso3116Alpha2Code, string iso3166Alpha3Code, string phoneCode,
        string phoneMask, string osm, bool needAutomaticUpdate, DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        Iso3116Alpha2Code = iso3116Alpha2Code;
        Iso3166Alpha3Code = iso3166Alpha3Code;
        PhoneCode = phoneCode;
        PhoneMask = phoneMask;
        Osm = osm;
        UpdatedAt = updatedAt;
        NeedAutomaticUpdate = needAutomaticUpdate;
        Geometry = geometry;
    }
    
#pragma warning disable CS8618
    private CountryEntity()
    {
    }
#pragma warning restore CS8618

    public int Id { get; }

    public CountryNameEntity Name { get; set; }

    public string Iso3116Alpha2Code { get; set; }

    public string Iso3166Alpha3Code { get; set; }

    public string PhoneCode { get; set; }

    public string PhoneMask { get; set; }
    
    public string Osm { get; set; }
    
    public bool NeedAutomaticUpdate { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    
    public CountryGeometryEntity Geometry { get; set; }
}