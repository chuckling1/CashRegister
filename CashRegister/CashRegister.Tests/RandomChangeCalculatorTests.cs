using CashRegister.Core.Enums;
using CashRegister.Core.Implementations;
using CashRegister.Core.Implementations.ChangeCalculators;
using CashRegister.Core.Interfaces;
using Moq;
using Xunit.Abstractions;

namespace CashRegister.Tests;

public class RandomChangeCalculatorTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private RandomChangeCalculator _calculator;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;
    private readonly Mock<IRandomGenerator> _randomGeneratorMock;

    public enum DenominationIndex
    {
        dollar = 0,
        quarter = 1,
        dime = 2,
        nickel = 3,
        penny = 4
    }
    
    
    public RandomChangeCalculatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _currencyRepositoryMock = CurrencyRepositoryMocks.GetUsdRepositoryMock();
        _randomGeneratorMock = new Mock<IRandomGenerator>();
        _calculator = new RandomChangeCalculator(_currencyRepositoryMock.Object, 3, _randomGeneratorMock.Object);
    }

    

    [Theory]
    [InlineData(3.00, 5.00, true)]
    [InlineData(6.00, 10.00, true)]
    [InlineData(2.99, 5.00, false)]
    [InlineData(4.00, 7.00, false)]
    public void DoesApplyToTransaction_ShouldReturnCorrectValue(decimal amountOwed, decimal amountPaid, bool expected)
    {
        // Act
        var result = _calculator.DoesApplyToTransaction(amountOwed, amountPaid);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(3.00, 5.00, 3, CurrencyType.USD, DenominationIndex.quarter, 8)]
    [InlineData(6.00, 10.00, 3, CurrencyType.USD, DenominationIndex.nickel, 80)]
    [InlineData(9.00, 15.00, 3, CurrencyType.USD, DenominationIndex.dollar, 6)]
    public void CalculateModifiedChange_ShouldReturnExpectedNumberOfDenomination(
        decimal amountOwed,
        decimal amountPaid, 
        decimal divisor,
        CurrencyType currencyType,
        DenominationIndex randomDenominationIndex, 
        int expectedNumberOfDenomination)
    {
        // Arrange
        _randomGeneratorMock.Setup(m => m.Next(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int)randomDenominationIndex);
        
        _calculator = new RandomChangeCalculator(_currencyRepositoryMock.Object, divisor, _randomGeneratorMock.Object);
        
        // Act
        var result = _calculator.Calculate(amountOwed, amountPaid, currencyType);

        // Assert
        Assert.Equal(expectedNumberOfDenomination, result.BillsAndCoins.Values.FirstOrDefault());
    }
}