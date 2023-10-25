using System.Text.Json.Serialization;

namespace ExternalTranslator.Models;

public class MyMemoryJson
{
    [JsonPropertyName("responseData")] public MyMemoryResponseDataJson ResponseData { get; init; } = null!;
}