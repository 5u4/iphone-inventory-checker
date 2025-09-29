namespace IPhoneStockChecker.Core.Settings;

public interface IWorkflowSettings
{
    TimeSpan CheckInterval { get; }
}

public record WorkflowSettings : IWorkflowSettings
{
    public required TimeSpan CheckInterval { get; init; }
}
