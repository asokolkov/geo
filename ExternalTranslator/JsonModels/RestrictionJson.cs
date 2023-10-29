using System.Text.Json.Serialization;
using ExternalTranslator.Enums;

namespace ExternalTranslator.JsonModels;

public class RestrictionJson
{
    [JsonPropertyName("type")] public RestrictionType Type { get; set; }
    [JsonPropertyName("currentAmount")] public int CurrentAmount { get; set; }
}