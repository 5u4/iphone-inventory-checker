using Common.ServiceConfigurations;
using IPhoneStockChecker.Core.ServiceConfigurations;
using IPhoneStockChecker.Core.Settings;
using IPhoneStockChecker.Notifiers.ServiceConfigurations;
using IPhoneStockChecker.Notifiers.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IPhoneStockChecker.Console.ServiceConfigurations;

public class ConsoleServiceComponent(IConfiguration configuration) : BaseServiceComponent
{
    protected override IReadOnlyList<Type> ExternalTypes => [typeof(ILogger<>)];

    protected override IReadOnlyList<BaseServiceComponent> Components =>
        [new StockCheckerServiceComponent(), new NotifierServiceComponent()];

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSettings<IBrowserSettings, BrowserSettings>(
            configuration.GetSection("Browser")
        );

        services.AddSettings<IInventoryPageSettings, InventoryPageSettings>(
            configuration.GetSection("InventoryPage")
        );

        services.AddSettings<IInventoryCheckerSettings, InventoryCheckerSettings>(
            configuration.GetSection("InventoryChecker")
        );

        services.AddSettings<IWorkflowSettings, WorkflowSettings>(
            configuration.GetSection("Workflow")
        );

        services.AddSettings<INotifierSettings, NtfyNotifierSettings>(
            configuration.GetSection("Notifier")
        );

        services.AddSingleton<IReadOnlyList<INotifierSettings>>(sp =>
            [sp.GetRequiredService<INotifierSettings>()]
        ); // TODO

        services.AddTransient<IConsoleRunner, ConsoleRunner>();
    }
}
