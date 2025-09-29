using IPhoneStockChecker.Core.Browsers;
using IPhoneStockChecker.Core.Checkers;
using IPhoneStockChecker.Core.Settings;
using Microsoft.Playwright;
using Polly;
using Polly.Retry;

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
    IInventoryChecker inventoryChecker,
    IScreenshotMaker screenshotMaker
) : IWorkflow
{
    public event EventHandler? InventoryFound;

    public async Task Run(CancellationToken ct = default)
    {
        using var playwright = await playwrightFactory.Create();

        await using var browser = await browserFactory.Create(playwright);

        var resiliencePipeline = CreateResiliencePipeline();

        await resiliencePipeline.ExecuteAsync(
            async cancellationToken =>
            {
                await Execute(browser, cancellationToken);
            },
            ct
        );
    }

    private async Task Execute(IBrowser browser, CancellationToken ct)
    {
        IPage? page = null;
        try
        {
            page = await browser.NewPageAsync();

            var inventoryPage = await inventoryPageFactory.Create(page);

            while (!ct.IsCancellationRequested)
            {
                var hasInventory = await inventoryChecker.CheckInventory(inventoryPage);

                if (hasInventory)
                {
                    InventoryFound?.Invoke(this, EventArgs.Empty);

                    await screenshotMaker.Screenshot(inventoryPage.Page, "InventoryFound");
                }

                // randomize by +-10%
                var randomizedInterval = TimeSpan.FromMilliseconds(
                    settings.CheckInterval.TotalMilliseconds
                        * (0.9 + Random.Shared.NextDouble() * 0.2)
                );

                await Task.Delay(randomizedInterval, ct);
            }
        }
        finally
        {
            if (page != null)
            {
                await page.CloseAsync();
            }
        }
    }

    private ResiliencePipeline CreateResiliencePipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(
                new RetryStrategyOptions
                {
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromMinutes(2),
                    MaxDelay = TimeSpan.FromMinutes(10),
                    UseJitter = true,
                }
            )
            .Build();
    }
}
