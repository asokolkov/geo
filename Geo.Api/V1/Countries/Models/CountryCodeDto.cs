using System.Text.Json.Serialization;

namespace Geo.Api.V1.Countries.Models;

public sealed class CountryCodeDto
{
    public CountryCodeDto(string iso3116Alpha2, string iso3166Alpha3)
    {
        Iso3116Alpha2 = iso3116Alpha2;
        Iso3166Alpha3 = iso3166Alpha3;
    }

    [JsonPropertyName("iso3116_alpha2")]
    public string Iso3116Alpha2 { get; }
    
    [JsonPropertyName("iso3166_alpha3")]
    public string Iso3166Alpha3 { get; }
}