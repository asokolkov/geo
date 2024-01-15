using System.Text.Json.Serialization;

namespace Geo.Api.V1;

public sealed record ResultDto<TResource>(
    [property: JsonPropertyName("status")] StatusDto Status
)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("result")] public TResource? Result { get; init; }
}