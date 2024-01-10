namespace Geo.Api.Client.Domain.Airports;

public sealed class AirportId
{
    public AirportId(string iata)
    {
        Value = iata;
    }

    public AirportId(int id)
    {
        Value = id.ToString();
    }
    
    public string Value { get; }
}