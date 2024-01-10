using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Geo.Api.V1;

[PublicAPI]
public sealed record SearchResultDto(
    [property: JsonPropertyName("status")] StatusDto Status,
    [property: JsonPropertyName("result")] IReadOnlyCollection<SearchResultElementDto> Result);