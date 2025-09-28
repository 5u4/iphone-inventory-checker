using IPhoneStockChecker.Notifiers.Ntfy;
using IPhoneStockChecker.Notifiers.Settings;

namespace IPhoneStockChecker.Notifiers.Core;

public interface INotifierFactory
{
    INotifier Create(INotifierSettings settings);
}

internal class NotifierFactory(INtfyNotifierFactory ntfyNotifierFactory) : INotifierFactory
{
    public INotifier Create(INotifierSettings settings)
    {
        return settings switch
        {
            INtfyNotifierSettings or NtfyNotifierSettings => ntfyNotifierFactory.Create(
                settings as INtfyNotifierSettings ?? throw new InvalidOperationException()
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(settings)),
        };
    }
}
