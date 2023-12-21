﻿using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

internal sealed class NominatimJsonElement
{
    [JsonPropertyName("osm_id")] public string OsmId { get; init; } = null!;
    [JsonPropertyName("address")] public Dictionary<string, string> Address { get; init; } = null!;
}