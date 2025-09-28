namespace IPhoneStockChecker.Notifiers.Core;

public record Notification
{
    public required string Message { get; init; }
    public required NotificationSeverity Severity { get; init; }
    public required IReadOnlyDictionary<NotifierProperty, object> Properties { get; init; }
}
