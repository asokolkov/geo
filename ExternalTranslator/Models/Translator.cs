using System.Text.Json.Serialization;

namespace ExternalTranslator.Models;

public class Translator
{
    [JsonPropertyName("id")] public string Id { get; init; } = null!;
    [JsonPropertyName("url")] public string Url { get; init; } = null!;
    [JsonPropertyName("timeCheckpoint")] public DateTimeOffset? TimeCheckpoint { get; set; }
    [JsonPropertyName("restrictions")] public List<Restriction> Restrictions { get; init; } = new();
}