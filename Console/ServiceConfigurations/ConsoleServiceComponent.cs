using IPhoneStockChecker.Console.Settings;
using IPhoneStockChecker.Core.ServiceConfigurations;
using Microsoft.Extensions.DependencyInjection;
using ServiceConfigurations;

namespace IPhoneStockChecker.Console.ServiceConfigurations;

public class ConsoleServiceComponent(IConsoleAppSettings settings) : BaseServiceComponent
{
    protected override IReadOnlyList<BaseServiceComponent> Components =>
        [new StockCheckerServiceComponent()];

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(settings.Browser);
        services.AddSingleton(settings.InventoryPage);
        services.AddSingleton(settings.InventoryChecker);
        services.AddSingleton(settings.Workflow);
    }
}
