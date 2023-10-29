using System.Text.Json.Serialization;

namespace ExternalTranslator.JsonModels;

public class TranslatorJson
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("restrictions")] public List<RestrictionJson> Restrictions  { get; set; } = new();
}