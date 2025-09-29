namespace IPhoneStockChecker.Core.Settings;

public interface IBrowserSettings
{
    bool Headless { get; }
    int TimeoutInMilliseconds { get; }
    float SlowMoInMilliseconds { get; }
}

public record BrowserSettings : IBrowserSettings
{
    public required bool Headless { get; init; }
    public required int TimeoutInMilliseconds { get; init; }
    public required float SlowMoInMilliseconds { get; init; }
}
