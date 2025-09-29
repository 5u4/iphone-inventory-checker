using IPhoneStockChecker.Core.Workflows;
using IPhoneStockChecker.Notifiers.Core;
using IPhoneStockChecker.Notifiers.Settings;
using Microsoft.Extensions.Logging;

namespace IPhoneStockChecker.Console;

public interface IConsoleRunner
{
    Task Run(CancellationToken ct = default);
}

internal class ConsoleRunner(
    ILogger<ConsoleRunner> logger,
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
            logger.LogInformation("Inventory found");
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

        logger.LogInformation("Started");

        notifyRunner.Enqueue(
            new Notification
            {
                Message = "Started",
                Severity = NotificationSeverity.Default,
                Properties = new Dictionary<NotifierProperty, object>
                {
                    [NotifierProperty.Tags] = new[] { "iphone" },
                },
            }
        );

        try
        {
            await workflow.Run(ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);

            notifyRunner.Enqueue(
                new Notification
                {
                    Message = e.Message,
                    Severity = NotificationSeverity.Max,
                    Properties = new Dictionary<NotifierProperty, object>
                    {
                        [NotifierProperty.Title] = "Error",
                        [NotifierProperty.Tags] = new[] { "x" },
                    },
                }
            );
        }
    }
}
