using Geo.Api.Extensions;
using Geo.Api.Repositories.Extensions;
using Microsoft.Extensions.Internal;

var builder = WebApplication.CreateBuilder();
builder.Logging.AddConsole();

builder.Services.AddLogging(config => config.AddConsole());
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
builder.Services.AddEfRepositories();
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper();
var app = builder.Build();
app.MapControllers();

await app.RunAsync();