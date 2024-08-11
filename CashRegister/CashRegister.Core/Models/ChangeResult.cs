namespace CashRegister.Core.Models;

public class ChangeResult(Dictionary<Denomination, int> billsAndCoins)
{
    public Dictionary<Denomination, int> BillsAndCoins { get; } = billsAndCoins;
    public decimal TotalValue => BillsAndCoins.Keys.Sum(d => d.Value * BillsAndCoins[d]);
    public string AsChangeSummaryString()
    {
        var orderedDenominations = BillsAndCoins
            .OrderByDescending(d => d.Key.Value)
            .Select(d =>
            {
                var name = d.Value == 1 
                    ? d.Key.Name 
                    : d.Key.PluralName;
                return $"{d.Value} {name}";
            });

        return string.Join(",", orderedDenominations);
    }
}

