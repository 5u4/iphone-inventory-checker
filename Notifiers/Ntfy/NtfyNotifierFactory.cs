using IPhoneStockChecker.Notifiers.Settings;
using ntfy;

namespace IPhoneStockChecker.Notifiers.Ntfy;

public interface INtfyNotifierFactory
{
    INtfyNotifier Create(INtfyNotifierSettings settings);
}

internal class NtfyNotifierFactory : INtfyNotifierFactory
{
    public INtfyNotifier Create(INtfyNotifierSettings settings)
    {
        var client = new Client();

        return new NtfyNotifier(settings, client);
    }
}
