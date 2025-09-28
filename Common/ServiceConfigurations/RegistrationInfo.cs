using Microsoft.Extensions.DependencyInjection;

namespace Common.ServiceConfigurations;

internal record RegistrationInfo
{
    public bool IsExternal { get; init; }
    public required ServiceDescriptor ServiceDescriptor { get; init; }
}
