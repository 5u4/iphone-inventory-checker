namespace IPhoneStockChecker.Core.Settings;

public interface IInventoryPageSettings
{
    string SpecPageUrl { get; }
}

public record InventoryPageSettings : IInventoryPageSettings
{
    public required string SpecPageUrl { get; init; }
}
