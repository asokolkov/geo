namespace Geo.Api.Client.Domain.Result;

public sealed class ResultError
{
    public ResultError(int statusCode)
    {
        StatusCode = statusCode;
    }
    
    public int StatusCode { get; }
    
    public string? Description { get; init; }
}