using ObjectsLoader.Extractors;
using ObjectsLoader.Services;

var client = new HttpClientWrapper();
var timezoneManager = new TimezoneManager();
var linker = new ModelsLinker();
using var translator = new MyMemoryTranslator(client);

var countriesExtractor = new CountriesExtractor(client, translator);
var regionsExtractor = new RegionsExtractor(client, translator);
var citiesExtractor = new CitiesExtractor(client, translator, timezoneManager);
var airportsExtractor = new AirportsExtractor(client, translator, timezoneManager);
var railwaysExtractor = new RailwaysExtractor(client, translator, timezoneManager);

var countries = await countriesExtractor.Extract();
var regions = await regionsExtractor.Extract();
var cities = await citiesExtractor.Extract();
var airports = await airportsExtractor.Extract();
var railways = await railwaysExtractor.Extract();

linker.Link(regions, countries);
linker.Link(cities, regions);
linker.Link(airports, cities);
linker.Link(railways, cities);