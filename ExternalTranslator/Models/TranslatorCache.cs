using System.Text.Json.Serialization;

namespace ExternalTranslator.Models;

public class TranslatorCache
{
    [JsonPropertyName("id")] public string Id { get; init; } = null!;
    [JsonPropertyName("currentCharsAmount")] public int CurrentCharsAmount { get; set; }
    [JsonPropertyName("currentQueriesAmount")] public int CurrentQueriesAmount { get; set; }
    [JsonPropertyName("timeCheckpoint")] public DateTimeOffset? TimeCheckpoint { get; set; }
}