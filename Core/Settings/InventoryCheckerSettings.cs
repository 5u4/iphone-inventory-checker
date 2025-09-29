namespace IPhoneStockChecker.Core.Settings;

public interface IInventoryCheckerSettings
{
    string ZipCode { get; }
}

public record InventoryCheckerSettings : IInventoryCheckerSettings
{
    public required string ZipCode { get; init; }
}
