using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Geo.Api.V1.Common.Models;

[PublicAPI]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LanguageDto : byte
{
    En = 1,
    Ru,
    Kz,
}
