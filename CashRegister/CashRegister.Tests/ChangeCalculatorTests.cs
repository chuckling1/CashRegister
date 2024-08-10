
using CashRegister.Core.Implementations;
using CashRegister.Core.Implementations.ChangeModifiers;
using CashRegister.Core.Interfaces;
using Moq;

namespace CashRegister.Tests;

public class ChangeCalculatorTests
{
    private readonly IEnumerable<IChangeModifier> _changeModifiers;
    private readonly Mock<IRandomGenerator> _randomGeneratorMock;
    private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
    private ChangeCalculator _changeCalculator;
    
    public ChangeCalculatorTests()
    {
        _randomGeneratorMock = new Mock<IRandomGenerator>();
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        var divisor = 3;
        //IEnumerable<IChangeModifier> changeModifiers = [new DivisibleByNumberRandomChangeModifier(_currencyRepositoryMock.Object, "USD", divisor, _randomGeneratorMock.Object)];
        //_changeCalculator = new ChangeCalculator(_currencyRepositoryMock.Object, "USD", changeModifiers);
    }

    [Fact]
    public void CalculateChange_ExactPayment_ReturnsEmptyList()
    {
        // Arrange
        var amountOwed = 5.00m;
        var amountPaid = 5.00m;

        // Act
        var result = _changeCalculator.CalculateChange(amountOwed, amountPaid);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void CalculateChange_PaymentWithChange_ReturnsCorrectDenominations()
    {
        // Arrange
        var amountOwed = 2.50m;
        var amountPaid = 5.00m;

        // Act
        var result = _changeCalculator.CalculateChange(amountOwed, amountPaid);

        // Assert
        Assert.Equal("2 dollars,2 quarters", result);
    }

    [Fact]
    public void CalculateChange_RandomizedChange_WhenDivisibleBy3_ReturnsValidButRandomDenominations()
    {
        // Arrange
        var amountOwed = 3.00m;
        var amountPaid = 5.00m;

        // Act
        var result = _changeCalculator.CalculateChange(amountOwed, amountPaid);

        // Assert
        // Since the result is random, you can't assert the exact denominations
        // Instead, verify that the total change is correct
        var expectedChangeAmount = amountPaid - amountOwed;
        //var actualChangeAmount = CalculateTotalChangeAmount(result);
        
        //Assert.Equal(expectedChangeAmount, actualChangeAmount);
    }

    private decimal CalculateTotalChangeAmount(List<string> changeList)
    {
        // Simplified example logic to calculate the total change amount from the result list
        decimal total = 0;

        foreach (var change in changeList)
        {
            if (change.Contains("dollar"))
            {
                var amount = int.Parse(change.Split(' ')[0]);
                total += amount * 1.00m;
            }
            else if (change.Contains("quarter"))
            {
                var amount = int.Parse(change.Split(' ')[0]);
                total += amount * 0.25m;
            }
            else if (change.Contains("dime"))
            {
                var amount = int.Parse(change.Split(' ')[0]);
                total += amount * 0.10m;
            }
            else if (change.Contains("nickel"))
            {
                var amount = int.Parse(change.Split(' ')[0]);
                total += amount * 0.05m;
            }
            else if (change.Contains("penny"))
            {
                var amount = int.Parse(change.Split(' ')[0]);
                total += amount * 0.01m;
            }
        }

        return total;
    }
}
