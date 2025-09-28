using IPhoneStockChecker.Notifiers.Core;
using IPhoneStockChecker.Notifiers.Settings;
using ntfy;
using ntfy.Requests;

namespace IPhoneStockChecker.Notifiers.Ntfy;

public interface INtfyNotifier : INotifier;

internal class NtfyNotifier(INtfyNotifierSettings settings, Client client) : INtfyNotifier
{
    public async Task Notify(Notification notification)
    {
        var priority = notification.Severity switch
        {
            NotificationSeverity.Default => PriorityLevel.Default,
            NotificationSeverity.High => PriorityLevel.High,
            NotificationSeverity.Max => PriorityLevel.Max,
            _ => throw new ArgumentOutOfRangeException(
                nameof(notification.Severity),
                notification.Severity,
                null
            ),
        };

        var title = notification.Properties.GetValueOrDefault(NotifierProperty.Title)?.ToString();
        // https://docs.ntfy.sh/emojis/
        var tags = notification.Properties.GetValueOrDefault(NotifierProperty.Tags);

        var sendingMessage = new SendingMessage
        {
            Priority = priority,
            Tags = tags as string[],
            Message = notification.Message,
            Title = title,
        };

        await client.Publish(settings.Topic, sendingMessage);
    }
}
