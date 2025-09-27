namespace IPhoneStockChecker.Core.Settings;

public interface IWorkflowSettings
{
    TimeSpan CheckInterval { get; }
}
