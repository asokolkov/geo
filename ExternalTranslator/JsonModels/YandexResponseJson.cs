using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class YandexResponseJson
{
    [JsonPropertyName("translations")] public List<Dictionary<string, string>> Translations { get; init; } = null!;
}