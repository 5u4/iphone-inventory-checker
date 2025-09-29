using IPhoneStockChecker.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Checkers;

public interface IInventoryChecker
{
    Task<bool> CheckInventory(InventoryPage page);
}

internal class InventoryChecker(
    ILogger<InventoryChecker> logger,
    IInventoryCheckerSettings settings,
    IScreenshotMaker screenshotMaker
) : IInventoryChecker
{
    public async Task<bool> CheckInventory(InventoryPage page)
    {
        logger.LogDebug("Focus postal code input");
        await page.Page.FocusAsync(PageSelectors.PostalCodeInput);
        await screenshotMaker.Screenshot(page.Page, "FocusPostalCodeInput");

        logger.LogDebug("Type zip code");
        await page.Page.Keyboard.TypeAsync($"{settings.ZipCode}\n");
        await screenshotMaker.Screenshot(page.Page, "TypeZipCode");

        await Task.Delay(TimeSpan.FromSeconds(1)); // TODO
        await page.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Task.Delay(TimeSpan.FromSeconds(5)); // TODO

        logger.LogDebug("Wait for load finish");
        await screenshotMaker.Screenshot(page.Page, "WaitForLoadFinish");

        var notAvailableCount = await page
            .Page.Locator(PageSelectors.ProductNotAvailable)
            .CountAsync();

        logger.LogDebug("Reset input");
        await page.Page.ClickAsync(PageSelectors.ResetInput);
        await screenshotMaker.Screenshot(page.Page, "ResetInput");

        return notAvailableCount == 0;
    }
}
