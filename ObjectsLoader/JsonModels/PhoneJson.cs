using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public sealed class PhoneJson
{
    [JsonPropertyName("code")] public required string Code { get; init; }
    [JsonPropertyName("mask")] public required string Mask { get; init; }
}