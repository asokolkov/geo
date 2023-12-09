using System.Text.Encodings.Web;
using System.Text.Json;
using ExternalTranslator.JsonModels;
using ExternalTranslator.Options;
using ExternalTranslator.Services;
using ExternalTranslator.Services.Impl;
using ExternalTranslator.Translators;
using ExternalTranslator.Translators.Impl;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Configuration.SetBasePath(environment.ContentRootPath);
builder.Configuration.AddJsonFile("Properties/appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"Properties/appsettings.{environment.EnvironmentName}.json", optional: true);

var myMemoryEnv = builder.Configuration.GetSection("MyMemoryClientOptions");
if (myMemoryEnv.GetChildren().Count() != 2)
{
    throw new NullReferenceException("MyMemoryClientOptions environment variable must have DetectLanguageApiKey and ApiUrl");
}
var yandexEnv = builder.Configuration.GetSection("YandexClientOptions");
if (yandexEnv.GetChildren().Count() != 3)
{
    throw new NullReferenceException("YandexClientOptions environment variable must have ApiUrl, ApiKey and ApiFolderId");
}
var loggingEnv = builder.Configuration.GetSection("Logging");
if (loggingEnv.GetChildren().Count() != 2)
{
    throw new NullReferenceException("Logging environment variable must have LogLevel and File");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

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

builder.Services.Configure<MyMemoryClientOptions>(myMemoryEnv);
builder.Services.Configure<YandexClientOptions>(yandexEnv);

builder.Services.AddScoped<IDistributedCache, InMemoryDistributedCache>();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<ITranslatorClient, MyMemoryClient>();
// builder.Services.AddScoped<ITranslatorClient, YandexClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();