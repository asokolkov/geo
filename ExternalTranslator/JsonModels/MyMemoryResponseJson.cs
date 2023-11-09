using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class MyMemoryResponseJson
{
    [JsonPropertyName("translatedText")] public string? TranslatedText { get; init; }
}