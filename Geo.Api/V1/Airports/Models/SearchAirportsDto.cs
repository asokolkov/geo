using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Geo.Api.V1.Airports.Models;

[PublicAPI]
public sealed class SearchAirportsDto
{
    public SearchAirportsDto(string term)
    {
        Term = term;
    }

    [JsonPropertyName("term")]
    public string Term { get; }
}