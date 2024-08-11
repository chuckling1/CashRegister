using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
using Microsoft.Extensions.Options;
namespace CashRegister.Core.Implementations;

public class CurrencyRepository(IOptions<CurrencyOptions> options) : ICurrencyRepository
{
    private readonly CurrencyOptions _options = options.Value;

    public Task<List<Denomination>?> GetDenominationsByCurrencyTypeAsync(CurrencyType currencyType)
    {
        var currencyConfig = _options.Currencies
            .FirstOrDefault(c => c.CurrencyType.Equals(currencyType));

        return Task.FromResult(currencyConfig != null 
            ? currencyConfig.Denominations 
            : []);
    }
}