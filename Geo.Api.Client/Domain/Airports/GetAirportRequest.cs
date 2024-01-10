namespace Geo.Api.Client.Domain.Airports;

public sealed class GetAirportRequest
{
    public GetAirportRequest(AirportId id)
    {
        Id = id;
    }
    
    public AirportId Id { get; }
}