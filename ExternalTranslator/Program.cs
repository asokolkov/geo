using ExternalTranslator.Options;
using ExternalTranslator.Services;
using ExternalTranslator.Services.Impl;
using ExternalTranslator.Translators;
using ExternalTranslator.Translators.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.Configure<MyMemoryClientOptions>(builder.Configuration.GetSection("MyMemoryClientOptions"));
builder.Services.Configure<YandexClientOptions>(builder.Configuration.GetSection("YandexClientOptions"));

builder.Services.AddScoped<IDistributedCache, InMemoryDistributedCache>();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<ITranslatorClient, MyMemoryClient>();
builder.Services.AddScoped<ITranslatorClient, YandexClient>();

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