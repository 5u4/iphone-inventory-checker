using Microsoft.Playwright;

namespace IPhoneStockChecker.Core.Checkers;

public record InventoryPage
{
    public required IPage Page { get; init; }
}
