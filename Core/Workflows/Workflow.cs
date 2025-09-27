using IPhoneStockChecker.Core.Browsers;
using IPhoneStockChecker.Core.Checkers;
using IPhoneStockChecker.Core.Settings;
using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Workflows;

public interface IWorkflow
{
    event EventHandler InventoryFound;
    Task Run(CancellationToken ct = default);
}

internal class Workflow(
    IWorkflowSettings settings,
    IPlaywrightFactory playwrightFactory,
    IBrowserFactory browserFactory,
    IInventoryPageFactory inventoryPageFactory,
    IInventoryChecker inventoryChecker
) : IWorkflow
{
    public event EventHandler? InventoryFound;

    public async Task Run(CancellationToken ct = default)
    {
        using var playwright = await playwrightFactory.Create();

        await using var browser = await browserFactory.Create(playwright);

        var page = await browser.NewPageAsync();

        var inventoryPage = await inventoryPageFactory.Create(page);

        while (!ct.IsCancellationRequested)
        {
            var hasInventory = await inventoryChecker.CheckInventory(inventoryPage);

            if (hasInventory)
            {
                InventoryFound?.Invoke(this, EventArgs.Empty);

                await inventoryPage.Page.ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        Path = $"InventoryFound.{DateTimeOffset.Now.ToUnixTimeSeconds()}.png",
                    }
                );
            }

            await Task.Delay(settings.CheckInterval, ct);
        }
    }
}
