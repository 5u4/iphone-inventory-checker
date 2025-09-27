using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Browsers;

public interface IPlaywrightFactory
{
    Task<IPlaywright> Create();
}

internal class PlaywrightFactory : IPlaywrightFactory
{
    public async Task<IPlaywright> Create()
    {
        return await Playwright.CreateAsync();
    }
}
