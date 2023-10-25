using System.Text.Json.Serialization;

namespace ExternalTranslator.Models;

public class MyMemoryResponseDataJson
{
    [JsonPropertyName("translatedText")] public string TranslatedText { get; init; } = null!;
}