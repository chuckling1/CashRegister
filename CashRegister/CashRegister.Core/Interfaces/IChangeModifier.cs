namespace CashRegister.Core.Interfaces;

public interface IChangeModifier
{
    bool DoesApplyToTransaction(decimal amountOwed, decimal amountPaid);
    Dictionary<string, int> CalculateModifiedChange(decimal amountOwed, decimal amountPaid);
}