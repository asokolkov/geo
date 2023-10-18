using ObjectsLoader.Models;

namespace ObjectsLoader.Services;

public class ModelsLinker
{
    public void Link(List<Region> regions, List<Country> countries)
    {
        foreach (var region in regions)
        {
            var countryIsoCode = region.IsoCode[..2];
            var countryId = countries.FirstOrDefault(x => x.Iso2 == countryIsoCode)?.Id;
            if (countryId is not null)
            {
                region.CountryId = (Guid)countryId;
            }
        }
    }

    public void Link(List<City> cities, List<Region> regions)
    {
        return;
    }

    public void Link(List<Airport> airports, List<City> countries)
    {
        return;
    }

    public void Link(List<Railway> railways, List<City> countries)
    {
        return;
    }
}