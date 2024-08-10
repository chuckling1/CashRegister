using CashRegister.Core.Interfaces;

namespace CashRegister.Core.Implementations;

public class ChangeCalculator(ICurrencyRepository currencyRepository, string currencyType, IEnumerable<IChangeModifier> changeModifiers) : IChangeCalculator
{
    // Get currency from currency repository
    
    
    public string CalculateChange(decimal amountOwed, decimal amountPaid)
    {
        var changeList = new List<string>();
        var changeAmount = amountPaid - amountOwed;
        var changeAmountRemaining = changeAmount;

        //currencyRepository.GetCurrency(currencyType);
        
        while(changeAmountRemaining > 0)
        {
            
        }

        return null;
    }
    
    private string BuildChangeString(Dictionary<string, int> change)
    {
        return string.Join(',', change);
    }
}