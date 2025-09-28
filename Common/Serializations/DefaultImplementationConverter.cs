using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Serializations;

public class DefaultImplementationConverter<TInterface, TImplementation> : JsonConverter<TInterface>
    where TImplementation : class, TInterface
{
    public override TInterface Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using var document = JsonDocument.ParseValue(ref reader);

        return JsonSerializer.Deserialize<TImplementation>(
            document.RootElement.GetRawText(),
            options
        )!;
    }

    public override void Write(
        Utf8JsonWriter writer,
        TInterface value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
