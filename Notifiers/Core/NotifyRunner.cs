using System.Threading.Channels;

namespace IPhoneStockChecker.Notifiers.Core;

public interface INotifyRunner
{
    void Enqueue(Notification notification);
    Task Run(INotifierProvider notifierProvider, CancellationToken ct = default);
}

internal class NotifyRunner : INotifyRunner
{
    private readonly Channel<Notification> channel = Channel.CreateUnbounded<Notification>();

    public void Enqueue(Notification notification)
    {
        if (!channel.Writer.TryWrite(notification))
        {
            throw new InvalidOperationException("Cannot enqueue notification - runner is disposed");
        }
    }

    public async Task Run(INotifierProvider notifierProvider, CancellationToken ct = default)
    {
        await foreach (var notification in channel.Reader.ReadAllAsync(ct))
        {
            await notifierProvider.NotifyAll(notification);
        }
    }
}
