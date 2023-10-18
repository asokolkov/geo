namespace Geo.Api.Extensions;

using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Countries;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql());
        services.AddScoped<ICountriesRepository, CountriesRepository>();
        return services;
    }
}