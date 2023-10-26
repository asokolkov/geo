using System.Text.Json.Serialization;
using ExternalTranslator.Enums;

namespace ExternalTranslator.Models;

public class Restriction
{
    [JsonPropertyName("type")] public RestrictionType Type { get; init; }
    [JsonPropertyName("maxAmount")] public int MaxAmount { get; init; }
    [JsonPropertyName("period")] public TimeSpan Period { get; init; }
    [JsonPropertyName("currentAmount")] public int CurrentAmount { get; set; }
    public bool LimitReached(int amount) => CurrentAmount + amount >= MaxAmount;
}