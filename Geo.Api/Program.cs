using Geo.Api.Extensions;

var builder = WebApplication.CreateBuilder();
builder.Logging.AddConsole();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddMediatR(_ => { });
builder.Services.AddRepositories();

var app = builder.Build();
app.MapControllers();

await app.RunAsync();