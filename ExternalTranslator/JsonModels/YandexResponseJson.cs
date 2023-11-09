using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class YandexResponseJson
{
    [JsonPropertyName("translations")] public IReadOnlyCollection<IReadOnlyDictionary<string, string>> Translations { get; init; } = null!;
}