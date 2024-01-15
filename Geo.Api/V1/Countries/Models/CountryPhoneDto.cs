using System.Text.Json.Serialization;

namespace Geo.Api.V1.Countries.Models;

public sealed class CountryPhoneDto
{
    public CountryPhoneDto(string code, string mask)
    {
        Code = code;
        Mask = mask;
    }

    [JsonPropertyName("code")]
    public string Code { get; }
    
    [JsonPropertyName("mask")]
    public string Mask { get; }
}