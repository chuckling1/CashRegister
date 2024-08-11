using CashRegister.Core.Enums;

namespace CashRegister.Core.Interfaces;

public interface IChangeCalculatorService
{
    string CalculateChange(decimal amountOwed, decimal amountPaid, CurrencyType currencyType);
    Task<Stream> CalculateBulkChangeAsync(Stream fileStream, CurrencyType currencyType);
}