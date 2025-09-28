using System.Collections.Immutable;
using IPhoneStockChecker.Notifiers.Settings;

namespace IPhoneStockChecker.Notifiers.Core;

public interface INotifierProviderFactory
{
    INotifierProvider Create(IReadOnlyList<INotifierSettings> settings);
}

internal class NotifierProviderFactory(INotifierFactory notifierFactory) : INotifierProviderFactory
{
    public INotifierProvider Create(IReadOnlyList<INotifierSettings> settings)
    {
        var notifiers = settings.Select(notifierFactory.Create).ToImmutableArray();

        return new NotifierProvider(notifiers);
    }
}
