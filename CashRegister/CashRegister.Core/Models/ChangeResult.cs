namespace CashRegister.Core.Models;

public class ChangeResult
{
    public Dictionary<string, int> Change { get; set; }
    public decimal CalculateChangeTotal => Change.Sum(x => x.Value);
}