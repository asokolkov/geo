namespace Geo.Api.Domain;

public sealed record SearchResult(
    Status Status,
    IReadOnlyCollection<SearchResultElement> Result);