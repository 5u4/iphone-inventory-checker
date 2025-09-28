using System.Text.Json.Serialization;
using Common.Serializations;

namespace IPhoneStockChecker.Notifiers.Settings;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(NtfyNotifierSettings), "Ntfy")]
public interface INotifierSettings;

[JsonConverter(typeof(DefaultImplementationConverter<INtfyNotifierSettings, NtfyNotifierSettings>))]
public interface INtfyNotifierSettings : INotifierSettings
{
    string Topic { get; }
}

public record NtfyNotifierSettings : INtfyNotifierSettings
{
    public required string Topic { get; init; }
}
