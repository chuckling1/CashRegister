namespace CashRegister.Core.Interfaces;

public interface IOptionalChangeCalculator: IChangeCalculator
{
    new bool DoesApplyToTransaction(decimal amountOwed, decimal amountPaid);
}