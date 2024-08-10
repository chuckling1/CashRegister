using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
using Microsoft.Extensions.Options;

namespace CashRegister.Core.Implementations;

public class CurrencyRepository(IOptions<CurrencyOptions> options) : ICurrencyRepository
{
    private readonly CurrencyOptions _options = options.Value;

    public Task<List<Denomination>> GetDenominationsByCurrencyTypeAsync(string? currencyType)
    {
        var currencyConfig = _options.Currencies
            .FirstOrDefault(c => c.CurrencyType.Equals(currencyType, StringComparison.OrdinalIgnoreCase));

        if (currencyConfig != null)
        {
            return Task.FromResult(currencyConfig.Denominations);
        }

        return Task.FromResult(new List<Denomination>());
    }
}