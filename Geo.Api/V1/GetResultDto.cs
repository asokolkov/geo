using System.Text.Json.Serialization;

namespace Geo.Api.V1;

public sealed record GetResultDto<TResource>(
    [property: JsonPropertyName("status")] StatusDto Status,
    [property: JsonPropertyName("result")] IReadOnlyCollection<TResource> Result
);