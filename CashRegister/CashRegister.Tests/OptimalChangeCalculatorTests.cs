using CashRegister.Core.Enums;
using CashRegister.Core.Implementations.ChangeCalculators;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
using Moq;
namespace CashRegister.Tests;

public class OptimalChangeCalculatorTests
{
    private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
    private OptimalChangeCalculator _changeCalculator;

    public OptimalChangeCalculatorTests()
    {
        _currencyRepositoryMock = CurrencyRepositoryMocks.GetUsdRepositoryMock();
        _changeCalculator = new OptimalChangeCalculator(_currencyRepositoryMock.Object);
    }

    [Fact]
    public void CalculateChange_ExactPayment_ReturnsEmptyChangeResult()
    {
        // Arrange
        var amountOwed = 5.00m;
        var amountPaid = 5.00m;
        var expectedChangeResult = new ChangeResult(new Dictionary<Denomination, int>());

        // Act
        var result = _changeCalculator.Calculate(amountOwed, amountPaid, CurrencyType.USD);

        // Assert
        Assert.Equal(expectedChangeResult.TotalValue, result.TotalValue);
        Assert.Equal(expectedChangeResult.AsChangeSummaryString(), result.AsChangeSummaryString());
    }

    [Fact]
    public void CalculateChange_PaymentWithChange_ReturnsCorrectDenominations()
    {
        // Arrange
        var amountOwed = 2.50m;
        var amountPaid = 5.00m;
        var expectedChangeResult = new ChangeResult(
            new Dictionary<Denomination, int>
            {
                { new Denomination("dollar", "dollars", 1.00m), 2 },
                { new Denomination("quarter", "quarters", 0.25m), 2 }
            });

        // Act
        var result = _changeCalculator.Calculate(amountOwed, amountPaid, CurrencyType.USD);

        // Assert
        Assert.Equal(expectedChangeResult.TotalValue, result.TotalValue);
        Assert.Equal(expectedChangeResult.AsChangeSummaryString(), result.AsChangeSummaryString());
    }

    [Fact]
    public void CalculateChange_PaymentWithChange_DifferentDenominations()
    {
        // Arrange
        var amountOwed = 1.30m;
        var amountPaid = 2.00m;
        var expectedChangeResult = new ChangeResult(
            new Dictionary<Denomination, int>
            {
                { new Denomination("quarter", "quarters", 0.25m), 2 },
                { new Denomination("dime", "dimes", 0.10m), 2 }
            });

        // Act
        var result = _changeCalculator.Calculate(amountOwed, amountPaid, CurrencyType.USD);

        // Assert
        Assert.Equal(expectedChangeResult.TotalValue, result.TotalValue);
        Assert.Equal(expectedChangeResult.AsChangeSummaryString(), result.AsChangeSummaryString());
    }
}
