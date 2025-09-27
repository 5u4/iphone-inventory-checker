using IPhoneStockChecker.Core.Settings;
using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Checkers;

public interface IInventoryPageFactory
{
    Task<InventoryPage> Create(IPage page);
}

internal class InventoryPageFactory(IInventoryPageSettings settings) : IInventoryPageFactory
{
    public async Task<InventoryPage> Create(IPage page)
    {
        await page.GotoAsync(settings.SpecPageUrl);

        await Task.Delay(TimeSpan.FromSeconds(3)); // TODO

        await page.Locator(PageSelectors.PickUpCheckInventory).ScrollIntoViewIfNeededAsync();

        await page.ClickAsync(PageSelectors.PickUpCheckInventory);

        return new InventoryPage { Page = page };
    }
}
