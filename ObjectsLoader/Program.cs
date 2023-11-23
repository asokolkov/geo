using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.Clients.Impl;
using ObjectsLoader.Extractors;
using ObjectsLoader.Extractors.Impl;
using ObjectsLoader.ExtractorsServices;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;
using ObjectsLoader.Services;
using ObjectsLoader.Services.Impl;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(config =>
        {
            config.AddSimpleConsole(options => options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ");
        });
        
        services.AddSingleton<INominatimClient, NominatimClient>();
        services.AddSingleton<IOsmClient, OsmClient>();
        services.AddSingleton<ITranslatorClient, TranslatorClient>();
        services.AddSingleton<ITimezoneManager, TimezoneManager>();
        services.AddSingleton<IDistributedCache, DistributedCache>();
        
        // services.AddSingleton<IExtractor<Country>, CountriesExtractor>();
        // services.AddSingleton<IExtractor<Region>, RegionsExtractor>();
        // services.AddSingleton<IExtractor<City>, CitiesExtractor>();
        services.AddSingleton<IExtractor<Airport>, AirportsExtractor>();
        // services.AddSingleton<IExtractor<Railway>, RailwaysExtractor>();
        
        // services.AddSingleton<CountriesService>();
        // services.AddSingleton<RegionsService>();
        // services.AddSingleton<CitiesService>();
        services.AddSingleton<AirportsService>();
        // services.AddSingleton<RailwaysService>();
        
        // services.AddCronJob<CountriesService>(config => { config.TimeZoneInfo = TimeZoneInfo.Local; config.CronExpression = CronExpression.EveryMinute; });
        // services.AddCronJob<RegionsService>(config => { config.TimeZoneInfo = TimeZoneInfo.Local; config.CronExpression = CronExpression.EveryMinute; });
        // services.AddCronJob<CitiesService>(config => { config.TimeZoneInfo = TimeZoneInfo.Local; config.CronExpression = CronExpression.EveryMinute; });
        services.AddCronJob<AirportsService>(config => { config.TimeZoneInfo = TimeZoneInfo.Local; config.CronExpression = CronExpression.EveryMinute; });
        // services.AddCronJob<RailwaysService>(config => { config.TimeZoneInfo = TimeZoneInfo.Local; config.CronExpression = CronExpression.EveryMinute; });
    })
    .Build()
    .RunAsync();