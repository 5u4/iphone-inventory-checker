using IPhoneStockChecker.Core.Workflows;
using IPhoneStockChecker.Notifiers.Core;
using IPhoneStockChecker.Notifiers.Settings;

namespace IPhoneStockChecker.Console;

public interface IConsoleRunner
{
    Task Run(CancellationToken ct = default);
}

internal class ConsoleRunner(
    IReadOnlyList<INotifierSettings> notifierSettings,
    INotifierProviderFactory notifierProviderFactory,
    INotifyRunnerFactory notifyRunnerFactory,
    IWorkflowFactory workflowFactory
) : IConsoleRunner
{
    public async Task Run(CancellationToken ct = default)
    {
        var notifierProvider = notifierProviderFactory.Create(notifierSettings);
        var notifyRunner = notifyRunnerFactory.Create();
        _ = notifyRunner.Run(notifierProvider, ct);

        var workflow = workflowFactory.Create();
        workflow.InventoryFound += (_, _) =>
        {
            notifyRunner.Enqueue(
                new Notification
                {
                    Message = "Found inventory",
                    Severity = NotificationSeverity.Max,
                    Properties = new Dictionary<NotifierProperty, object>
                    {
                        [NotifierProperty.Title] = "Inventory Found",
                        [NotifierProperty.Tags] = new[] { "iphone" },
                    },
                }
            );
        };

        await workflow.Run(ct);
    }
}
