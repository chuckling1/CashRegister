using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
namespace CashRegister.Core.Implementations;

public class ChangeCalculatorService(
    CurrencyType currencyType,
    IChangeCalculator defaultChangeCalculator,
    IEnumerable<IOptionalChangeCalculator>? optionalChangeCalculators = null)
{
    private readonly IEnumerable<IOptionalChangeCalculator> _optionalChangeCalculators = optionalChangeCalculators ?? [];
    
    public string CalculateChange(decimal amountOwed, decimal amountPaid)
    {
        foreach (var calculator in _optionalChangeCalculators)
        {
            if (calculator.DoesApplyToTransaction(amountOwed, amountPaid))
            {
                return calculator
                    .Calculate(amountOwed, amountPaid, currencyType)
                    .AsChangeSummaryString();
            }
        }

        // Fallback to the default calculator if no optional calculators apply
        return defaultChangeCalculator
            .Calculate(amountOwed, amountPaid, currencyType)
            .AsChangeSummaryString();
    }
}