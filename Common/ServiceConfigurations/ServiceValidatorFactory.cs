namespace Common.ServiceConfigurations;

internal class ServiceValidatorFactory
{
    public IServiceValidator Create(IReadOnlyDictionary<Type, RegistrationInfo> registrationInfoMap)
    {
        return new ServiceValidator(registrationInfoMap);
    }
}
