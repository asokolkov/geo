using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class LogJson
{
    [JsonPropertyName("timestamp")] public required string Timestamp { get; init; }
    [JsonPropertyName("log_level")] public required string LogLevel { get; init; }
    [JsonPropertyName("log_name")] public required string LogName { get; init; }
    [JsonPropertyName("event_id")] public int EventId { get; init; }
    [JsonPropertyName("message")] public required string Message { get; init; }    
    [JsonPropertyName("exception")] public string? Exception { get; init; }
}