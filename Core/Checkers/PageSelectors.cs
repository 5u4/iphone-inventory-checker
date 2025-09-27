namespace IPhoneStockChecker.Core.Checkers;

internal static class PageSelectors
{
    public const string PickUpCheckInventory = "//button[@data-autom='productLocatorTriggerLink']";

    public const string PostalCodeInput = "//input[@data-autom='zipCode']";

    public const string ProductNotAvailable =
        "//span[text()='Not available today at 5 nearest stores.']";

    public const string ResetInput = "//button[span[text()='Reset']]";
}
