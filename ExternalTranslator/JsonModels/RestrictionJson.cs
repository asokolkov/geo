using System.Text.Json.Serialization;
using ExternalTranslator.Enums;

namespace ExternalTranslator.JsonModels;

public class RestrictionJson
{
    [JsonPropertyName("type")] public RestrictionType Type { get; init; }
    [JsonPropertyName("timeCheckpoint")] public DateTimeOffset? TimeCheckpoint { get; init; }
    [JsonPropertyName("currentAmount")] public int CurrentAmount { get; init; }
}