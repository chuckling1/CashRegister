using CashRegister.Core.Models;

namespace CashRegister.Core.Interfaces;

public interface ICurrencyRepository
{
    Task<List<Denomination>> GetDenominationsByCurrencyTypeAsync(string? currencyType);
}
