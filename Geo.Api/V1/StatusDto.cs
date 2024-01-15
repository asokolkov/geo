using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Geo.Api.V1;

[PublicAPI]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusDto : byte
{
    Error = 1,
    Ok = 2
}
