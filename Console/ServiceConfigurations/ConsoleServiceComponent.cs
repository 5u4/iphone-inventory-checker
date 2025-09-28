using System.Collections.Immutable;
using Common.ServiceConfigurations;
using IPhoneStockChecker.Console.Settings;
using IPhoneStockChecker.Core.ServiceConfigurations;
using IPhoneStockChecker.Notifiers.ServiceConfigurations;
using IPhoneStockChecker.Notifiers.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace IPhoneStockChecker.Console.ServiceConfigurations;

public class ConsoleServiceComponent(IConsoleAppSettings settings) : BaseServiceComponent
{
    protected override IReadOnlyList<BaseServiceComponent> Components =>
        [new StockCheckerServiceComponent(), new NotifierServiceComponent()];

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(settings.Browser);
        services.AddSingleton(settings.InventoryPage);
        services.AddSingleton(settings.InventoryChecker);
        services.AddSingleton(settings.Workflow);
        services.AddSingleton<IReadOnlyList<INotifierSettings>>(
            settings.Notifiers.ToImmutableArray()
        );

        services.AddTransient<IConsoleRunner, ConsoleRunner>();
    }
}
