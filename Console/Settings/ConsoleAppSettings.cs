using IPhoneStockChecker.Core.Settings;
using IPhoneStockChecker.Notifiers.Settings;

namespace IPhoneStockChecker.Console.Settings;

public interface IConsoleAppSettings
{
    IBrowserSettings Browser { get; }
    IInventoryPageSettings InventoryPage { get; }
    IInventoryCheckerSettings InventoryChecker { get; }
    IWorkflowSettings Workflow { get; }
    IEnumerable<INotifierSettings> Notifiers { get; }
}

public record ConsoleAppSettings : IConsoleAppSettings
{
    public required IBrowserSettings Browser { get; init; }
    public required IInventoryPageSettings InventoryPage { get; init; }
    public required IInventoryCheckerSettings InventoryChecker { get; init; }
    public required IWorkflowSettings Workflow { get; init; }
    public required IEnumerable<INotifierSettings> Notifiers { get; init; }
}
