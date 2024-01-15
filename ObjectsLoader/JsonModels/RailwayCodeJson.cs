using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class RailwayCodeJson
{
    [JsonPropertyName("express3")] public required string Express3 { get; init; }
}