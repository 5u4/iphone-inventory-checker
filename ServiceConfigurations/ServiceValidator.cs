using System.Reflection;

namespace ServiceConfigurations;

internal interface IServiceValidator
{
    void Validate(out IReadOnlySet<Type> touched);
}

internal class ServiceValidator(IReadOnlyDictionary<Type, RegistrationInfo> registrationInfoMap)
    : IServiceValidator
{
    public void Validate(out IReadOnlySet<Type> touched)
    {
        var mutableTouched = new HashSet<Type>();
        touched = mutableTouched;

        foreach (var x in registrationInfoMap)
        {
            ValidateRegistrationInfo(x.Key, x.Value, out var registrationInfoTouched);

            mutableTouched.UnionWith(registrationInfoTouched);
        }
    }

    private void ValidateRegistrationInfo(
        Type type,
        RegistrationInfo info,
        out IReadOnlySet<Type> touched
    )
    {
        var mutableTouched = new HashSet<Type>();
        touched = mutableTouched;

        if (info.IsExternal)
        {
            return;
        }

        if (info.ServiceDescriptor.ImplementationType is not null)
        {
            ValidateTypeBasedRegistration(
                type,
                info.ServiceDescriptor.ImplementationType,
                mutableTouched
            );
        }
        else if (info.ServiceDescriptor.ImplementationFactory != null)
        {
            // Factory-based registration: services.AddTransient<IService>(sp => new ServiceImpl())
            // We can't validate factory registrations at compile time, so we skip validation
            // The factory is responsible for ensuring dependencies are available
        }
        else if (info.ServiceDescriptor.ImplementationInstance != null)
        {
            // Instance-based registration: services.AddSingleton<IService>(instance)
            // Instance is already created, so no validation needed
        }
        else
        {
            throw new InvalidOperationException(
                $"Service registration for '{type.FullName}' has no implementation type, factory, or instance"
            );
        }
    }

    private void ValidateTypeBasedRegistration(
        Type serviceType,
        Type implementationType,
        HashSet<Type> mutableTouched
    )
    {
        var isValid = false;
        var errorMessages = new List<string>();

        foreach (var ctor in implementationType.GetConstructors())
        {
            if (!ValidateCtor(serviceType, ctor, out var errorMessage, out var ctorTouched))
            {
                errorMessages.Add(errorMessage);
            }
            else
            {
                isValid = true;
                mutableTouched.UnionWith(ctorTouched);

                break;
            }
        }

        if (!isValid)
        {
            throw new InvalidOperationException(string.Join('\n', errorMessages));
        }
    }

    private bool ValidateCtor(
        Type type,
        ConstructorInfo ctor,
        out string errorMessage,
        out IReadOnlySet<Type> touched
    )
    {
        var mutableTouched = new HashSet<Type>();
        touched = mutableTouched;

        errorMessage = string.Empty;

        var parameterTypes = ctor.GetParameters().Select(p => p.ParameterType).ToHashSet();

        var pendingParameterTypes = new HashSet<Type>();

        foreach (var parameterType in parameterTypes)
        {
            if (registrationInfoMap.ContainsKey(parameterType))
            {
                continue;
            }

            // for close generic, try finding the open generic
            if (parameterType is { IsGenericType: true, ContainsGenericParameters: false })
            {
                var openGeneric = parameterType.GetGenericTypeDefinition();

                if (registrationInfoMap.ContainsKey(openGeneric))
                {
                    pendingParameterTypes.Add(openGeneric);

                    continue;
                }
            }

            pendingParameterTypes.Clear();

            errorMessage =
                $"Trying to resolve type '{type.FullName}' but failed to get '{parameterType.FullName}'";

            return false;
        }

        mutableTouched.UnionWith(pendingParameterTypes);
        mutableTouched.UnionWith(parameterTypes);

        return true;
    }
}
