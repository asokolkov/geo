using ObjectsLoader.Models;

namespace ObjectsLoader.Services;

public class ModelsLinker
{
    public void Link(List<Region> regions, List<Country> countries)
    {
        foreach (var region in regions)
        {
            var countryIsoCode = region.IsoCode[..2];
            var country = countries.FirstOrDefault(x => x.Iso2 == countryIsoCode);
            if (country is null)
            {
                continue;
            }
            region.CountryId = country.Id;
        }
    }

    public void Link(List<City> cities, List<Region> regions)
    {
        foreach (var city in cities)
        {
            var region = regions.FirstOrDefault(x => x.IsoCode == city.RegionIsoCode);
            if (region is null)
            {
                continue;
            }
            city.RegionId = region.Id;
            city.CountryId = region.CountryId;
        }
    }

    public void Link(List<Airport> airports, List<City> cities)
    {
        foreach (var airport in airports)
        {
            var city = cities.FirstOrDefault(x => x.OsmId == airport.CityOsmId);
            if (city is null)
            {
                continue;
            }
            airport.CityId = city.Id;
            airport.RegionId = city.RegionId;
            airport.CountryId = city.CountryId;
        }
    }

    public void Link(List<Railway> railways, List<City> cities)
    {
        foreach (var railway in railways)
        {
            var city = cities.FirstOrDefault(x => x.OsmId == railway.CityOsmId);
            if (city is null)
            {
                continue;
            }
            railway.CityId = city.Id;
            railway.RegionId = city.RegionId;
            railway.CountryId = city.CountryId;
        }
    }
}