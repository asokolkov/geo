using System.Globalization;
using System.Net;

namespace ObjectsLoader.Clients;

public class NominatimClient
{
    private const string Url = "https://nominatim.openstreetmap.org/reverse?format=json&lat={0}&lon={1}";
    private readonly HttpClient client = new();
    
    public NominatimClient()
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
    }
    
    public async Task<string?> Fetch(double latitude, double longitude)
    {
        var stringLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var stringLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        var query = string.Format(Url, stringLatitude, stringLongitude);
        var response = await client.GetAsync(query);
        return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : null;
    }
}