using System.Text.Json.Serialization;

namespace ObjectsLoader.JsonModels;

public class JsonResponse<T>
{
    [JsonPropertyName("status")] public string Status { get; init; }
    [JsonPropertyName("result")] public T Model { get; init; }
}