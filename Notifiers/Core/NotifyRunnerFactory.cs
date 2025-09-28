namespace IPhoneStockChecker.Notifiers.Core;

public interface INotifyRunnerFactory
{
    INotifyRunner Create();
}

internal class NotifyRunnerFactory : INotifyRunnerFactory
{
    public INotifyRunner Create()
    {
        return new NotifyRunner();
    }
}
