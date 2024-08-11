using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
using Moq;

namespace CashRegister.Tests;

public static class CurrencyRepositoryMocks
{
    public static Mock<ICurrencyRepository> GetUsdRepositoryMock()
    {
        var mock = new Mock<ICurrencyRepository>();
        mock.Setup(m => m.GetDenominationsByCurrencyTypeAsync(It.IsAny<CurrencyType>()))
            .ReturnsAsync(new List<Denomination>
            {
                new Denomination ("dollar", "dollars", 1.00m),
                new Denomination ("quarter", "quarters", 0.25m),
                new Denomination ("dime", "dimes", 0.10m),
                new Denomination ("nickel", "nickels", 0.05m),
                new Denomination ("penny", "pennies", 0.01m)
            });
        return mock;
    }
}