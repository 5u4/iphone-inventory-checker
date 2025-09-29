using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.ServiceConfigurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettings<TInterface, TImplementation>(
        this IServiceCollection services,
        IConfigurationSection section
    )
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.Configure<TImplementation>(section);

        services.AddSingleton<TInterface>(sp =>
            sp.GetRequiredService<IOptions<TImplementation>>().Value
        );

        return services;
    }
}
