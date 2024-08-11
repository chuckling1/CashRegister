using CashRegister.Core.Models;
namespace CashRegister.Tests;

public class ChangeSummaryStringTests
{
    [Fact]
    public void ToString_SingleDenomination_SingularForm()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("dollar", "dollars", 1.00m), 1 },
            { new Denomination("quarter", "quarters", 0.25m), 1 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var summary = changeResult.AsChangeSummaryString();

        // Assert
        Assert.Equal("1 dollar,1 quarter", summary);
    }

    [Fact]
    public void ToString_MultipleDenominations_PluralForm()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("dollar", "dollars", 1.00m), 2 },
            { new Denomination("quarter", "quarters", 0.25m), 3 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var summary = changeResult.AsChangeSummaryString();

        // Assert
        Assert.Equal("2 dollars,3 quarters", summary);
    }

    [Fact]
    public void ToString_MixedDenominations_CorrectPluralization()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>
        {
            { new Denomination("penny", "pennies", 0.01m), 1 },
            { new Denomination("dollar", "dollars", 1.00m), 2 },
            { new Denomination("quarter", "quarters", 0.25m), 1 },
            { new Denomination("dime", "dimes", 0.10m), 2 }
        };
        var changeResult = new ChangeResult(denominations);

        // Act
        var summary = changeResult.AsChangeSummaryString();

        // Assert
        Assert.Equal("2 dollars,1 quarter,2 dimes,1 penny", summary);
    }

    [Fact]
    public void ToString_NoDenominations_EmptyString()
    {
        // Arrange
        var denominations = new Dictionary<Denomination, int>();
        var changeResult = new ChangeResult(denominations);

        // Act
        var summary = changeResult.AsChangeSummaryString();

        // Assert
        Assert.Equal(string.Empty, summary);
    }
}
