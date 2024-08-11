using System.Text.Json.Serialization;
using CashRegister.Core.Enums;

namespace CashRegister.Core.Models;

public class CurrencyOptions
{
    [JsonConverter(typeof(CurrencyTypeConverter))]
    public List<Currency> Currencies { get; set; } = new();
}