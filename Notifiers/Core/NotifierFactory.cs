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
        if (settings is INtfyNotifierSettings ntfySettings)
        {
            return ntfyNotifierFactory.Create(ntfySettings);
        }

        throw new ArgumentOutOfRangeException(
            nameof(settings),
            $"Unsupported notifier settings type: {settings.GetType()}"
        );
    }
}
