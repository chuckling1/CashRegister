using CashRegister.Core.Enums;
using CashRegister.Core.Models;
namespace CashRegister.Core.Interfaces;

public interface IChangeCalculator
{
    ChangeResult Calculate(decimal amountOwed, decimal amountPaid, CurrencyType currencyType);
}