using Geo.Api.Client.Providers;

namespace Geo.Api.Client;

public interface IGeoClient
{
    IAirportsProvider Airports { get; }
    
    ICitiesProvider Cities { get; }
    
    ICountriesProvider Countries { get; }
    
    IRailwayStationsProvider RailwayStations { get; }
    
    IRegionsProvider Regions { get; }
}