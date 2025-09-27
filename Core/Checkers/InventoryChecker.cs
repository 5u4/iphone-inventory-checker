using IPhoneStockChecker.Core.Settings;
using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Checkers;

public interface IInventoryChecker
{
    Task<bool> CheckInventory(InventoryPage page);
}

internal class InventoryChecker(IInventoryCheckerSettings settings) : IInventoryChecker
{
    public async Task<bool> CheckInventory(InventoryPage page)
    {
        await page.Page.FocusAsync(PageSelectors.PostalCodeInput);

        await page.Page.Keyboard.TypeAsync($"{settings.ZipCode}\n");

        await Task.Delay(TimeSpan.FromSeconds(1)); // TODO

        await page.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Task.Delay(TimeSpan.FromSeconds(5)); // TODO

        var notAvailableCount = await page
            .Page.Locator(PageSelectors.ProductNotAvailable)
            .CountAsync();

        await page.Page.ClickAsync(PageSelectors.ResetInput);

        return notAvailableCount == 0;
    }
}
