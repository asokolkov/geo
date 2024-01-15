using ObjectsLoader.Clients;
using ObjectsLoader.Extractors;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;

namespace ObjectsLoader.ScheduledService;

public class ScheduleService : CronJobService
{
    private readonly IExtractor<Airport> airportsExtractor;
    private readonly IExtractor<Metro> metrosExtractor;
    private readonly IExtractor<Railway> railwaysExtractor;
    private readonly IExtractor<City> citiesExtractor;
    private readonly IExtractor<Region> regionsExtractor;
    private readonly IExtractor<Country> countriesExtractor;
    private readonly ISenderClient client;
    
    public ScheduleService(IExtractor<Metro> metrosExtractor, IExtractor<Railway> railwaysExtractor, IExtractor<Airport> airportsExtractor, IExtractor<City> citiesExtractor, IExtractor<Region> regionsExtractor, IExtractor<Country> countriesExtractor, ISenderClient client, IScheduleConfig<ScheduleService> config) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.metrosExtractor = metrosExtractor;
        this.railwaysExtractor = railwaysExtractor;
        this.airportsExtractor = airportsExtractor;
        this.citiesExtractor = citiesExtractor;
        this.regionsExtractor = regionsExtractor;
        this.countriesExtractor = countriesExtractor;
        this.client = client;
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var countries = await countriesExtractor.Extract();
        var countriesResponses = new List<CountryJson>();
        foreach (var country in countries)
        {
            var countryJson = new CountryJson
            {
                Id = Guid.NewGuid(),
                Code = new CountryCodeJson
                {
                    Iso2 = country.Iso3116Alpha2,
                    Iso3 = country.Iso3116Alpha3
                },
                Name = new NameJson
                {
                    Ru = country.NameRu,
                    En = country.NameEn
                },
                Geometry = new GeometryJson
                {
                    Latitude = 0,
                    Longitude = 0
                },
                Phone = new PhoneJson
                {
                    Code = country.PhoneCode,
                    Mask = country.PhoneMask
                },
                Osm = country.Osm,
                NeedToUpdate = false,
                LastUpdate = DateTimeOffset.Now
            };
            var response = await client.Send("/country", countryJson);
            if (response is not null)
            {
                countriesResponses.Add(response);
            }
        }
        
        var regions = await regionsExtractor.Extract();
        var regionsResponses = new List<RegionJson>();
        foreach (var region in regions)
        {
            var country = countriesResponses.Where(x => x.Code.Iso2 == region.CountryIso2Code).Select(x => x).FirstOrDefault();
            if (country is null)
            {
                continue;
            }
            
            var regionJson = new RegionJson
            {
                Name = new NameJson
                {
                    Ru = region.NameRu,
                    En = region.NameEn
                },
                LocationComponents = new RegionLocationComponentsJson
                {
                    CountryId = country.Id
                },
                Geometry = new GeometryJson
                {
                    Latitude = 0,
                    Longitude = 0
                },
                Osm = region.Osm,
                NeedToUpdate = false,
                LastUpdate = DateTimeOffset.Now
            };
            var response = await client.Send("/region", regionJson);
            if (response is not null)
            {
                regionsResponses.Add(response);
            }
        }
        
        var cities = await citiesExtractor.Extract();
        var citiesResponses = new List<CityJson>();
        foreach (var city in cities)
        {
            var regionOsmId = regions.FirstOrDefault(x => x.Iso == city.RegionIsoCode)?.Osm;
            if (regionOsmId is null)
            {
                continue;
            }
            var region = regionsResponses.FirstOrDefault(x => x.Osm == regionOsmId);
            if (region is null)
            {
                continue;
            }
            
            var cityJson = new CityJson
            {
                Code = new CodeJson
                {
                    En = ""
                },
                Name = new NameJson
                {
                    Ru = city.NameRu,
                    En = city.NameEn
                },
                LocationComponentsJson = new CityLocationComponentsJson
                {
                    RegionId = region.Id,
                    CountryId = region.LocationComponents.CountryId
                },
                Geometry = new GeometryJson
                {
                    Latitude = city.Latitude,
                    Longitude = city.Longitude
                },
                Osm = city.Osm,
                NeedToUpdate = false,
                LastUpdate = DateTimeOffset.Now
            };
            var response = await client.Send("/city", cityJson);
            if (response is not null)
            {
                citiesResponses.Add(response);
            }
        }
        
        var airports = await airportsExtractor.Extract();
        foreach (var airport in airports)
        {
            var city = citiesResponses.FirstOrDefault(x => x.Osm == airport.CityOsmId);
            if (city is null)
            {
                continue;
            }
            
            var airportJson = new AirportJson
            {
                Code = new CodeJson
                {
                    En = airport.IataEn,
                },
                Name = new NameJson
                {
                    Ru = airport.NameRu,
                    En = airport.NameEn
                },
                LocationComponents = new LocationComponentsJson
                {
                    CityId = city.Id,
                    RegionId = city.LocationComponentsJson.RegionId,
                    CountryId = city.LocationComponentsJson.CountryId
                },
                Geometry = new GeometryJson
                {
                    Latitude = airport.Latitude,
                    Longitude = airport.Longitude
                },
                Osm = airport.Osm,
                NeedToUpdate = false,
                LastUpdate = DateTimeOffset.Now
            };
            await client.Send("/airport", airportJson);
        }
        
        var railways = await railwaysExtractor.Extract();
        foreach (var railway in railways)
        {
            var city = citiesResponses.FirstOrDefault(x => x.Osm == railway.CityOsmId);
            if (city is null)
            {
                continue;
            }
            
            var railwayJson = new RailwayJson
            {
                Code = new RailwayCodeJson
                {
                    Express3 = railway.Express3Code,
                },
                Name = new NameJson
                {
                    Ru = railway.NameRu,
                    En = railway.NameEn
                },
                LocationComponents = new LocationComponentsJson
                {
                    CityId = city.Id,
                    RegionId = city.LocationComponentsJson.RegionId,
                    CountryId = city.LocationComponentsJson.CountryId
                },
                Geometry = new GeometryJson
                {
                    Latitude = railway.Latitude,
                    Longitude = railway.Longitude
                },
                Osm = railway.Osm,
                NeedToUpdate = false,
                LastUpdate = DateTimeOffset.Now
            };
            await client.Send("/rail_station", railwayJson);
        }
        
        var metros = await metrosExtractor.Extract();
        foreach (var metro in metros)
        {
            var city = citiesResponses.FirstOrDefault(x => x.Osm == metro.CityOsmId);
            if (city is null)
            {
                continue;
            }
            
            var metroJson = new MetroJson
            {
                StationName = new NameJson
                {
                    Ru = metro.StationNameRu,
                    En = metro.StationNameEn
                },
                LineName = new NameJson
                {
                    Ru = metro.LineNameRu,
                    En = metro.LineNameEn
                },
                LocationComponents = new LocationComponentsJson
                {
                    CityId = city.Id,
                    RegionId = city.LocationComponentsJson.RegionId,
                    CountryId = city.LocationComponentsJson.CountryId
                },
                Geometry = new GeometryJson
                {
                    Latitude = metro.Latitude,
                    Longitude = metro.Longitude
                },
                Osm = metro.Osm,
                NeedToUpdate = false,
                LastUpdate = DateTimeOffset.Now
            };
            await client.Send("/metro", metroJson);
        }
    }
}