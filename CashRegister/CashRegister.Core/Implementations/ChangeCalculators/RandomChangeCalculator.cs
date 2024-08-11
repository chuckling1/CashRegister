using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
namespace CashRegister.Core.Implementations.ChangeCalculators;

public class RandomChangeCalculator(
    ICurrencyRepository currencyRepository,
    decimal divisor,
    IRandomGenerator randomGenerator)
    : IOptionalChangeCalculator
{
    private readonly ICurrencyRepository _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    private readonly decimal _divisor = divisor <= 0 ? throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be greater than zero.") : divisor;
    private readonly IRandomGenerator _randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));

    public bool DoesApplyToTransaction(decimal amountOwed, decimal amountPaid)
    {
        return amountOwed % _divisor == 0;
    }

    public ChangeResult Calculate(decimal amountOwed, decimal amountPaid, CurrencyType currencyType)
    {
        var initialChangeAmount = amountPaid - amountOwed;
        var changeAmountRemaining = initialChangeAmount;
        var currencyDenominations = _currencyRepository.GetDenominationsByCurrencyTypeAsync(currencyType).Result;
        var change = new Dictionary<Denomination, int>();

        while (changeAmountRemaining > 0)
        {
            if (currencyDenominations == null)
            {
                throw new Exception("Currency denominations not found.");
            }
            
            var randomIndex = _randomGenerator.Next(0, currencyDenominations.Count);
            var denomination = currencyDenominations[randomIndex];
            if (denomination.Value > changeAmountRemaining)
            {
                continue;
            }

            changeAmountRemaining -= denomination.Value;

            if (!change.TryAdd(denomination, 1))
            {
                change[denomination]++;
            }
        }

        return new ChangeResult(change);
    }
}