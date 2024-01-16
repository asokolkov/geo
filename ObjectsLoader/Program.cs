using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ObjectsLoader.Clients;
using ObjectsLoader.Clients.Impl;
using ObjectsLoader.Extractors;
using ObjectsLoader.Extractors.Impl;
using ObjectsLoader.JsonModels;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;
using ObjectsLoader.Services;
using ObjectsLoader.Services.Impl;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("Properties/appsettings.json", true, true);

var loggingEnv = builder.Configuration.GetSection("Logging");
if (loggingEnv.GetChildren().Count() != 2)
{
    throw new NullReferenceException("Logging environment variable must have LogLevel and File");
}

builder.Services.AddLogging(config => {
    config.AddFile(loggingEnv, options =>
    {
        options.FormatLogEntry = message =>
        {
            var log = new LogJson
            {
                Timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                LogLevel = message.LogLevel.ToString(),
                LogName = message.LogName,
                EventId = message.EventId.Id,
                Message = message.Message,
                Exception = message.Exception?.ToString()
            };
            var serializationOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            return JsonSerializer.Serialize(log, serializationOptions);
        };
    });
});

builder.Services.AddSingleton<INominatimClient, NominatimClient>();
builder.Services.AddSingleton<IOsmClient, OsmClient>();
builder.Services.AddSingleton<ITranslatorClient, TranslatorClient>();
builder.Services.AddSingleton<ITimezoneManager, TimezoneManager>();
builder.Services.AddSingleton<IDistributedCache, DistributedCache>();
builder.Services.AddSingleton<ISenderClient, SenderClient>();

builder.Services.AddSingleton<IExtractor<Country>, CountriesExtractor>();
builder.Services.AddSingleton<IExtractor<Region>, RegionsExtractor>();
builder.Services.AddSingleton<IExtractor<City>, CitiesExtractor>();
builder.Services.AddSingleton<IExtractor<Airport>, AirportsExtractor>();
builder.Services.AddSingleton<IExtractor<Railway>, RailwaysExtractor>();
builder.Services.AddSingleton<IExtractor<Metro>, MetroExtractor>();
        
builder.Services.AddSingleton<ScheduleService>();
builder.Services.AddCronJob<ScheduleService>(config => { config.TimeZoneInfo = TimeZoneInfo.Local; config.CronExpression = CronExpression.EveryMinute; });

var app = builder.Build();

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

app.Run();