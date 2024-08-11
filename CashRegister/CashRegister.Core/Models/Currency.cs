namespace CashRegister.Core.Models;

public class Currency
{
    public string? CurrencyType { get; set; }
    public List<Denomination>? Denominations { get; set; }
}