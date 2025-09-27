using IPhoneStockChecker.Core.Settings;

namespace IPhoneStockChecker.Console.Settings;

public interface IConsoleAppSettings
{
    IBrowserSettings Browser { get; }
    IInventoryPageSettings InventoryPage { get; }
    IInventoryCheckerSettings InventoryChecker { get; }
    IWorkflowSettings Workflow { get; }
}
