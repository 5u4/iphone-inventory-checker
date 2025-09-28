namespace IPhoneStockChecker.Notifiers.Core;

public interface INotifier
{
    Task Notify(Notification notification);
}
