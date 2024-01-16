﻿using System.Text.Json.Serialization;
using ObjectsLoader.Extensions;

namespace ObjectsLoader.JsonModels;

public sealed class CountryJson
{
    [JsonConverter(typeof(IntToStringConverter))] [JsonPropertyName("id")] public string Id { get; init; }
    [JsonPropertyName("code")] public required CountryCodeJson Code { get; init; }
    [JsonPropertyName("name")] public required NameJson Name { get; init; }   
    [JsonPropertyName("geometry")] public required GeometryJson Geometry { get; init; }
    [JsonPropertyName("phone")] public required PhoneJson Phone { get; init; }
    [JsonPropertyName("osm")] public required string Osm { get; init; }
    [JsonPropertyName("need_to_update")] public required bool NeedToUpdate { get; init; }
    [JsonPropertyName("last_update")] public required DateTimeOffset LastUpdate { get; init; }
}