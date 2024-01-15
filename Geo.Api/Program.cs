using Geo.Api.Extensions;
using Geo.Api.Repositories.Extensions;
using Microsoft.Extensions.Internal;

var builder = WebApplication.CreateBuilder();
builder.Configuration.AddUserSecrets<Program>();
builder.Logging.AddConsole();

builder.Services.AddLogging(config => config.AddConsole());
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddEfRepositories(builder.Configuration);
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


await app.RunAsync();