using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class YandexRequestJson
{
    [JsonPropertyName("targetLanguageCode")] public string Target { get; init; } = null!;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Source { get; init; }
    [JsonPropertyName("texts")] public string Text { get; init; } = null!;
    [JsonPropertyName("folder_id")] public string FolderId { get; init; } = null!;
}