using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Checkers;

public interface IScreenshotMaker
{
    Task Screenshot(IPage page, string tag);
}

internal class ScreenshotMaker : IScreenshotMaker
{
    public async Task Screenshot(IPage page, string tag)
    {
        var path = $"./logs/screenshot.{tag}.png";

        await page.ScreenshotAsync(new PageScreenshotOptions { Path = path });
    }
}
