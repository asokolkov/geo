using Geo.Api.Client.Internal;
using Geo.Api.Client.Internal.Airports;
using Geo.Api.Client.Internal.Cities;
using Geo.Api.Client.Providers;

namespace Geo.Api.Client;

internal sealed class GeoClient : IGeoClient
{
    public GeoClient(GeoClientOptions options)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = options.ApiUrl
        };

        Airports = new AirportsProvider(httpClient);
        Cities = new CitiesProvider();
    }
    
    public IAirportsProvider Airports { get; }
    public ICitiesProvider Cities { get; }
    public ICountriesProvider Countries { get; }
    public IRailwayStationsProvider RailwayStations { get; }
    public IRegionsProvider Regions { get; }
}