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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddLogging(config => {
    config.AddFile(builder.Configuration.GetSection("Logging"), options =>
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

builder.Services.Configure<MyMemoryClientOptions>(builder.Configuration.GetSection("MyMemoryClientOptions"));
builder.Services.Configure<YandexClientOptions>(builder.Configuration.GetSection("YandexClientOptions"));

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