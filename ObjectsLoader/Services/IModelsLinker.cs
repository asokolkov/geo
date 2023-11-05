using ObjectsLoader.Models;

namespace ObjectsLoader.Services;

public interface IModelsLinker
{
    public void Link(List<Region> regions, List<Country> countries);
    public void Link(List<City> cities, List<Region> regions);
    public void Link(List<Airport> airports, List<City> cities);
    public void Link(List<Railway> railways, List<City> cities);
}