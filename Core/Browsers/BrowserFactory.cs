using IPhoneStockChecker.Core.Settings;
using Microsoft.Playwright;
using Soenneker.Playwrights.Extensions.Stealth;

namespace IPhoneStockChecker.Core.Browsers;

public interface IBrowserFactory
{
    Task<IBrowser> Create(IPlaywright playwright);
}

internal class BrowserFactory(IBrowserSettings settings) : IBrowserFactory
{
    public async Task<IBrowser> Create(IPlaywright playwright)
    {
        return await playwright.LaunchStealthChromium(
            new BrowserTypeLaunchOptions
            {
                Headless = settings.Headless,
                Timeout = settings.TimeoutInMilliseconds,
            }
        );
    }
}
