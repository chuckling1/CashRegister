namespace CashRegister.Core.Interfaces;

public interface IChangeCalculator
{
    string CalculateChange(decimal amountOwed, decimal amountPaid);
}