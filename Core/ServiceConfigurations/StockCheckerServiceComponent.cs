using Common.ServiceConfigurations;
using IPhoneStockChecker.Core.Browsers;
using IPhoneStockChecker.Core.Checkers;
using IPhoneStockChecker.Core.Settings;
using IPhoneStockChecker.Core.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IPhoneStockChecker.Core.ServiceConfigurations;

public class StockCheckerServiceComponent : BaseServiceComponent
{
    protected override IReadOnlyList<Type> ExternalTypes =>
        [
            typeof(ILogger<>),
            typeof(IBrowserSettings),
            typeof(IInventoryPageSettings),
            typeof(IInventoryCheckerSettings),
            typeof(IWorkflowSettings),
        ];

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPlaywrightFactory, PlaywrightFactory>();
        services.AddSingleton<IBrowserFactory, BrowserFactory>();
        services.AddSingleton<IScreenshotMaker, ScreenshotMaker>();
        services.AddSingleton<IInventoryPageFactory, InventoryPageFactory>();
        services.AddSingleton<IInventoryChecker, InventoryChecker>();
        services.AddSingleton<IWorkflowFactory, WorkflowFactory>();
    }
}
