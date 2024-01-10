namespace Geo.Api.Client.Domain.Airports;

public sealed class SearchAirportsRequest
{
    public SearchAirportsRequest(Language language, int pageNumber, int pageSize)
    {
        Language = language;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    
    public string? Query { get; init; }
    
    public Language Language { get; }
    
    public int PageNumber { get; }
    
    public int PageSize { get; }
}