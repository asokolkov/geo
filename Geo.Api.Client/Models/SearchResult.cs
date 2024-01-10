using System.Text.Json.Serialization;

namespace Geo.Api.Client.Models;

public sealed class SearchResult<TValue>
{
    public SearchResult(IReadOnlyCollection<TValue> foundEntities, int totalCount, int count, bool hasNext)
    {
        FoundEntities = foundEntities;
        TotalCount = totalCount;
        Count = count;
        HasNext = hasNext;
    }

    [JsonPropertyName("found_entities")]
    public IReadOnlyCollection<TValue> FoundEntities { get; }

    [JsonPropertyName("total_count")]
    public int TotalCount { get; }

    [JsonPropertyName("count")]
    public int Count { get; }

    [JsonPropertyName("has_next")]
    public bool HasNext { get; }
}