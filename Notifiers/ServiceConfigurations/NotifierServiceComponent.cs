using Common.ServiceConfigurations;
using IPhoneStockChecker.Notifiers.Core;
using IPhoneStockChecker.Notifiers.Ntfy;
using Microsoft.Extensions.DependencyInjection;

namespace IPhoneStockChecker.Notifiers.ServiceConfigurations;

public class NotifierServiceComponent : BaseServiceComponent
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<INotifierFactory, NotifierFactory>();
        services.AddSingleton<INotifierProviderFactory, NotifierProviderFactory>();

        services.AddSingleton<INtfyNotifierFactory, NtfyNotifierFactory>();

        services.AddSingleton<INotifyRunnerFactory, NotifyRunnerFactory>();
    }
}
