using IPhoneStockChecker.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Checkers;

public interface IInventoryPageFactory
{
    Task<InventoryPage> Create(IPage page);
}

internal class InventoryPageFactory(
    ILogger<InventoryPageFactory> logger,
    IInventoryPageSettings settings,
    IScreenshotMaker screenshotMaker
) : IInventoryPageFactory
{
    public async Task<InventoryPage> Create(IPage page)
    {
        logger.LogDebug("Goto page {Page}", settings.SpecPageUrl);
        await page.GotoAsync(settings.SpecPageUrl);
        await Task.Delay(TimeSpan.FromSeconds(3)); // TODO
        await screenshotMaker.Screenshot(page, "GoToPage");

        logger.LogDebug("Scroll into pick up check inventory view");
        await page.Locator(PageSelectors.PickUpCheckInventory).ScrollIntoViewIfNeededAsync();
        await screenshotMaker.Screenshot(page, "PickUpCheckInventory");

        logger.LogDebug("Click on pick up check inventory");
        await page.ClickAsync(PageSelectors.PickUpCheckInventory);
        await screenshotMaker.Screenshot(page, "ClickPickUpCheckInventory");

        return new InventoryPage { Page = page };
    }
}
