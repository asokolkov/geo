using Geo.Api.V1.Countries.Mapping;

namespace Geo.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CountriesProfile>();
        });
        return services;
    }
}