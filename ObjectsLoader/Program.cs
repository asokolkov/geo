using ObjectsLoader.Clients;
using ObjectsLoader.Clients.Impl;
using ObjectsLoader.Extractors;
using ObjectsLoader.Extractors.Impl;
using ObjectsLoader.Services;

var osmClient = new OsmClient();
var nominatimClient = new NominatimClient();
var translatorClient = new TranslatorClient();
var timezoneManager = new TimezoneManager();
var linker = new ModelsLinker();

var countriesExtractor = new CountriesExtractor(osmClient, translatorClient);
var regionsExtractor = new RegionsExtractor(osmClient, translatorClient);
var citiesExtractor = new CitiesExtractor(osmClient, nominatimClient, translatorClient, timezoneManager);
var airportsExtractor = new AirportsExtractor(osmClient, nominatimClient, translatorClient, timezoneManager);
var railwaysExtractor = new RailwaysExtractor(osmClient, nominatimClient, translatorClient, timezoneManager);

var countries = await countriesExtractor.Extract();
var regions = await regionsExtractor.Extract();
var cities = await citiesExtractor.Extract();
var airports = await airportsExtractor.Extract();
var railways = await railwaysExtractor.Extract();

linker.Link(regions, countries);
linker.Link(cities, regions);
linker.Link(airports, cities);
linker.Link(railways, cities);