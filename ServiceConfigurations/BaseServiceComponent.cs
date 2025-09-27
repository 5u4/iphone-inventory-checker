using Microsoft.Extensions.DependencyInjection;

namespace ServiceConfigurations;

public abstract class BaseServiceComponent
{
    protected virtual IReadOnlyList<Type> ExternalTypes { get; } = [];
    protected virtual IReadOnlyList<BaseServiceComponent> Components { get; } = [];
    protected abstract void ConfigureServices(IServiceCollection services);

    public void Configure(IServiceCollection services)
    {
        ConfigureServices(services);
        ConfigureComponents(services);
    }

    public void Verify()
    {
        var services = new ServiceCollection();
        Configure(services);

        var registrationInfoMap = services.ToDictionary(
            x => x.ServiceType,
            x => new RegistrationInfo { ServiceDescriptor = x }
        );

        RegisterExternalTypes(registrationInfoMap, ExternalTypes);

        var serviceValidator = new ServiceValidatorFactory().Create(registrationInfoMap);

        serviceValidator.Validate(out var touched);

        foreach (var x in ExternalTypes)
        {
            if (!touched.Contains(x))
            {
                throw new InvalidOperationException(
                    $"External type '{x.FullName}' is not required for service component"
                );
            }
        }
    }

    private void ConfigureComponents(IServiceCollection services)
    {
        foreach (var component in Components)
        {
            component.Configure(services);
        }
    }

    private static void RegisterExternalTypes(
        Dictionary<Type, RegistrationInfo> registrationInfoMap,
        IReadOnlyList<Type> externalTypes
    )
    {
        foreach (var externalType in externalTypes)
        {
            if (registrationInfoMap.ContainsKey(externalType))
            {
                continue;
            }

            var serviceDescriptor = externalType.IsGenericTypeDefinition
                ? ServiceDescriptor.Singleton(externalType, externalType)
                : ServiceDescriptor.Singleton(
                    externalType,
                    _ =>
                        throw new InvalidOperationException(
                            $"External service {externalType.FullName} should be provided by external container"
                        )
                );

            registrationInfoMap[externalType] = new RegistrationInfo
            {
                IsExternal = true,
                ServiceDescriptor = serviceDescriptor,
            };
        }
    }
}
