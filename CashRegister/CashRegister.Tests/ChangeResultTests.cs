using CashRegister.Core.Models;
using Xunit;

namespace CashRegister.Tests;

public class ChangeResultTests
{
    [Fact]
    public void TotalValue_NoDenominations_ReturnsZero()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>();
        var changeResult = new ChangeResult(denominations);

        // Act
        var totalValue = changeResult.TotalValue;

        // Assert
        Assert.Equal(0m, totalValue);
    }

    [Fact]
    public void TotalValue_SingleDenomination_ReturnsCorrectValue()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("dollar", "dollars", 1.00m), 2 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var totalValue = changeResult.TotalValue;

        // Assert
        Assert.Equal(2.00m, totalValue);
    }

    [Fact]
    public void TotalValue_MultipleDenominations_ReturnsSumOfValues()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("dollar", "dollars", 1.00m), 2 },
            { new Denomination("quarter", "quarters", 0.25m), 3 },
            { new Denomination("dime", "dimes", 0.10m), 4 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var totalValue = changeResult.TotalValue;

        // Assert
        Assert.Equal(2.00m + (3 * 0.25m) + (4 * 0.10m), totalValue);
    }

    [Fact]
    public void TotalValue_IncludesAllDenominations()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("penny", "pennies", 0.01m), 100 },
            { new Denomination("nickel", "nickels", 0.05m), 20 },
            { new Denomination("quarter", "quarters", 0.25m), 4 },
            { new Denomination("dollar", "dollars", 1.00m), 1 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var totalValue = changeResult.TotalValue;

        // Assert
        Assert.Equal((100 * 0.01m) + (20 * 0.05m) + (4 * 0.25m) + 1.00m, totalValue);
    }

    [Fact]
    public void TotalValue_SinglePenny_ReturnsCorrectValue()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("penny", "pennies", 0.01m), 1 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var totalValue = changeResult.TotalValue;

        // Assert
        Assert.Equal(0.01m, totalValue);
    }
}
