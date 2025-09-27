using IPhoneStockChecker.Core.Browsers;
using IPhoneStockChecker.Core.Checkers;
using IPhoneStockChecker.Core.Settings;

namespace IPhoneStockChecker.Core.Workflows;

public interface IWorkflowFactory
{
    IWorkflow Create();
}

internal class WorkflowFactory(
    IWorkflowSettings settings,
    IPlaywrightFactory playwrightFactory,
    IBrowserFactory browserFactory,
    IInventoryPageFactory inventoryPageFactory,
    IInventoryChecker inventoryChecker
) : IWorkflowFactory
{
    public IWorkflow Create()
    {
        return new Workflow(
            settings,
            playwrightFactory,
            browserFactory,
            inventoryPageFactory,
            inventoryChecker
        );
    }
}
