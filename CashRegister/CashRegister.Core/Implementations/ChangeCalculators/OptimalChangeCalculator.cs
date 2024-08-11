using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
namespace CashRegister.Core.Implementations.ChangeCalculators;

public class OptimalChangeCalculator(ICurrencyRepository currencyRepository): IChangeCalculator
{
    public ChangeResult Calculate(decimal amountOwed, decimal amountPaid, CurrencyType currencyType)
    {
        var initialChangeAmount = amountPaid - amountOwed;
        var changeAmountRemaining = initialChangeAmount;
        var denominations = currencyRepository
            .GetDenominationsByCurrencyTypeAsync(currencyType)
            .Result;

        if (denominations == null)
        {
            throw new Exception("Currency type not found");
        }

        var currencyDenominations = denominations
            .OrderByDescending(d => d.Value); // Sort by value, highest first
        
        var change = new Dictionary<Denomination, int>();

        foreach (var denomination in currencyDenominations)
        {
            if (denomination.Value > changeAmountRemaining)
            {
                continue;
            }

            var count = (int)(changeAmountRemaining / denomination.Value);
            changeAmountRemaining -= denomination.Value * count;

            if (count > 0)
            {
                change[denomination] = count;
            }

            if (changeAmountRemaining == 0)
            {
                break;
            }
        }

        return new ChangeResult(change);
    }
}