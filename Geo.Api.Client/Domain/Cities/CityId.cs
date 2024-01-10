namespace Geo.Api.Client.Domain.Cities;

public sealed class CityId
{
    public CityId(int id)
    {
        Value = id.ToString();
    }
    
    public string Value { get; }
}