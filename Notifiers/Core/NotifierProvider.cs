namespace IPhoneStockChecker.Notifiers.Core;

public interface INotifierProvider
{
    Task NotifyAll(Notification notification);
}

internal class NotifierProvider(IReadOnlyList<INotifier> notifiers) : INotifierProvider
{
    public async Task NotifyAll(Notification notification)
    {
        await Task.WhenAll(notifiers.Select(x => x.Notify(notification)));
    }
}
