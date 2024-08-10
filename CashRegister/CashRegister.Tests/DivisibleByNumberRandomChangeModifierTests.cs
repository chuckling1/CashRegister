using CashRegister.Core.Implementations;
using CashRegister.Core.Implementations.ChangeModifiers;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;
using Moq;
using Xunit.Abstractions;

namespace CashRegister.Tests;

public class DivisibleByNumberRandomChangeModifierTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private DivisibleByNumberRandomChangeModifier _modifier;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;
    private Mock<IRandomGenerator> _randomGeneratorMock;

    public enum DenominationIndex
    {
        dollar = 0,
        quarter = 1,
        dime = 2,
        nickel = 3,
        penny = 4
    }
    
    
    public DivisibleByNumberRandomChangeModifierTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _currencyRepositoryMock.Setup(m => m.GetDenominationsByCurrencyTypeAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<Denomination>
            {
                new Denomination { Name = "dollar", Value = 1.00m },
                new Denomination { Name = "quarter", Value = 0.25m },
                new Denomination { Name = "dime", Value = 0.10m },
                new Denomination { Name = "nickel", Value = 0.05m },
                new Denomination { Name = "penny", Value = 0.01m }
            });
        
        _randomGeneratorMock = new Mock<IRandomGenerator>();
        _randomGeneratorMock.Setup(m => m.Next(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(3);
        //_modifier = new DivisibleByNumberRandomChangeModifier(_currencyRepositoryMock.Object, "USD", 3, _randomGeneratorMock.Object);
    }
    
    [Theory]
    [InlineData(3.00, 5.00, true)]
    [InlineData(6.00, 10.00, true)]
    [InlineData(2.99, 5.00, false)]
    [InlineData(4.00, 7.00, false)]
    public void DoesApplyToTransaction_ShouldReturnCorrectValue(decimal amountOwed, decimal amountPaid, bool expected)
    {
        // Act
        var result = _modifier.DoesApplyToTransaction(amountOwed, amountPaid);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(3.00, 5.00, 3, "USD", DenominationIndex.quarter, 8)]
    [InlineData(6.00, 10.00, 3, "USD", DenominationIndex.nickel, 80)]
    [InlineData(9.00, 15.00, 3, "USD", DenominationIndex.dollar, 6)]
    public void CalculateModifiedChange_ShouldReturnExpectedNumberOfDenomination(
        decimal amountOwed,
        decimal amountPaid, 
        decimal divisor,
        string currencyType,
        DenominationIndex randomDenominationIndex, 
        int expectedNumberOfDenomination)
    {
        // Arrange
        _randomGeneratorMock.Setup(m => m.Next(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int)randomDenominationIndex);
        
        _modifier = new DivisibleByNumberRandomChangeModifier.Builder()
            .WithCurrencyRepository(_currencyRepositoryMock.Object)
            .WithCurrencyType(currencyType)
            .WithDivisor(divisor)
            .WithRandomGenerator(_randomGeneratorMock.Object)
            .Build();
        
        // Act
        var result = _modifier.CalculateModifiedChange(amountOwed, amountPaid);

        // Assert
        Assert.Equal(expectedNumberOfDenomination, result.Values.FirstOrDefault());
    }
    
    [Theory]
    [InlineData(3.00, 5.67, 3, "USD", DenominationIndex.quarter, 8)]
    [InlineData(6.00, 10.00, 3, "USD", DenominationIndex.nickel, 80)]
    [InlineData(9.00, 15.00, 3, "USD", DenominationIndex.dollar, 6)]
    public void CalculateModifiedChange_WithRealRandomNumbers_ShouldReturnExpectedNumberOfDenomination(
        decimal amountOwed,
        decimal amountPaid, 
        decimal divisor,
        string currencyType,
        DenominationIndex randomDenominationIndex, 
        int expectedNumberOfDenomination)
    {
        // Arrange
        _randomGeneratorMock.Setup(m => m.Next(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int)randomDenominationIndex);
        
        _modifier = new DivisibleByNumberRandomChangeModifier.Builder()
            .WithCurrencyRepository(_currencyRepositoryMock.Object)
            .WithCurrencyType(currencyType)
            .WithDivisor(divisor)
            .WithRandomGenerator(new RandomGenerator())
            .Build();
        
        // Act
        var result = _modifier.CalculateModifiedChange(amountOwed, amountPaid);

        // Assert
        _testOutputHelper.WriteLine($"amountOwed: {amountOwed}, amountPaid: {amountPaid}, result: {result.Values}");
    }
}