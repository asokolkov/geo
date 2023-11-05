using ObjectsLoader.Clients;
using ObjectsLoader.Extractors;
using ObjectsLoader.Services;

var osmClient = new OsmClient();
var nominatimClient = new NominatimClient();
var client = new HttpClientWrapper();
var timezoneManager = new TimezoneManager();
var linker = new ModelsLinker();
using var translator = new MyMemoryTranslator(client);

var countriesExtractor = new CountriesExtractor(osmClient, translator);
var regionsExtractor = new RegionsExtractor(osmClient, translator);
var citiesExtractor = new CitiesExtractor(osmClient, nominatimClient, translator, timezoneManager);
var airportsExtractor = new AirportsExtractor(osmClient, nominatimClient, translator, timezoneManager);
var railwaysExtractor = new RailwaysExtractor(osmClient, nominatimClient, translator, timezoneManager);

var countries = await countriesExtractor.Extract();
var regions = await regionsExtractor.Extract();
var cities = await citiesExtractor.Extract();
var airports = await airportsExtractor.Extract();
var railways = await railwaysExtractor.Extract();

linker.Link(regions, countries);
linker.Link(cities, regions);
linker.Link(airports, cities);
linker.Link(railways, cities);