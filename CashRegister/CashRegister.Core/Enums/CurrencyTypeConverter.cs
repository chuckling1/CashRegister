using System.Text.Json;
using System.Text.Json.Serialization;
namespace CashRegister.Core.Enums;

public class CurrencyTypeConverter : JsonConverter<CurrencyType>
{
    public override CurrencyType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<CurrencyType>(reader.GetString() ?? throw new InvalidOperationException(), true);
    }

    public override void Write(Utf8JsonWriter writer, CurrencyType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}