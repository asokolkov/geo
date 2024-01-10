using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Geo.Api.V1.Common.Models;

[PublicAPI]
public sealed class SearchResultDto<T>
{
    public SearchResultDto(IReadOnlyCollection<T> foundObjects, int totalCount, int count, bool hasNext)
    {
        FoundObjects = foundObjects;
        TotalCount = totalCount;
        Count = count;
        HasNext = hasNext;
    }

    [JsonPropertyName("found_objects")] public IReadOnlyCollection<T> FoundObjects { get; }

    [JsonPropertyName("total_count")] public int TotalCount { get; }

    [JsonPropertyName("count")] public int Count { get; }

    [JsonPropertyName("has_next")] public bool HasNext { get; }
}