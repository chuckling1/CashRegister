using CashRegister.Core.Interfaces;

namespace CashRegister.Core.Implementations.ChangeModifiers;

public class DivisibleByNumberRandomChangeModifier : IChangeModifier
{
    private readonly ICurrencyRepository? _currencyRepository;
    private readonly string? _currencyType;
    private readonly decimal _divisor;
    private readonly IRandomGenerator? _randomGenerator;

    // Constructor made private to enforce the use of the Builder
    private DivisibleByNumberRandomChangeModifier(
        ICurrencyRepository? currencyRepository,
        string? currencyType,
        decimal divisor,
        IRandomGenerator? randomGenerator)
    {
        _currencyRepository = currencyRepository;
        _currencyType = currencyType;
        _divisor = divisor;
        _randomGenerator = randomGenerator;
    }

    public bool DoesApplyToTransaction(decimal amountOwed, decimal amountPaid)
    {
        return amountOwed % _divisor == 0;
    }

    public Dictionary<string, int> CalculateModifiedChange(decimal amountOwed, decimal amountPaid)
    {
        if (!DoesApplyToTransaction(amountOwed, amountPaid))
        {
            return new Dictionary<string, int>();
        }

        var initialChangeAmount = amountPaid - amountOwed;
        var changeAmountRemaining = initialChangeAmount;
        var currencyDenominations = _currencyRepository.GetDenominationsByCurrencyTypeAsync(_currencyType).Result;
        var change = new Dictionary<string, int>();

        while (changeAmountRemaining > 0)
        {
            var randomIndex = _randomGenerator.Next(0, currencyDenominations.Count);
            var denomination = currencyDenominations[randomIndex];
            if (denomination.Value > changeAmountRemaining)
            {
                continue;
            }

            changeAmountRemaining -= denomination.Value;

            if (!change.TryAdd(denomination.Name, 1))
            {
                change[denomination.Name]++;
            }
        }

        return change;
    }

    // Nested Builder class
    public class Builder
    {
        private ICurrencyRepository? _currencyRepository;
        private string? _currencyType;
        private decimal _divisor;
        private IRandomGenerator? _randomGenerator;

        public Builder WithCurrencyRepository(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            return this;
        }

        public Builder WithCurrencyType(string currencyType)
        {
            _currencyType = currencyType ?? throw new ArgumentNullException(nameof(currencyType));
            return this;
        }

        public Builder WithDivisor(decimal divisor)
        {
            if (divisor <= 0)
                throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be greater than zero.");
            
            _divisor = divisor;
            return this;
        }

        public Builder WithRandomGenerator(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            return this;
        }

        public DivisibleByNumberRandomChangeModifier Build()
        {
            // All non-nullable fields are set before the object is created
            return new DivisibleByNumberRandomChangeModifier(
                _currencyRepository, 
                _currencyType, 
                _divisor, 
                _randomGenerator);
        }
    }
}