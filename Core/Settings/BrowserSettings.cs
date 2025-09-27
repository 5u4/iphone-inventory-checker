namespace IPhoneStockChecker.Core.Settings;

public interface IBrowserSettings
{
    bool Headless { get; }
    int TimeoutInMilliseconds { get; }
}
