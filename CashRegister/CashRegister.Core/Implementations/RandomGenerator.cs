using CashRegister.Core.Interfaces;
namespace CashRegister.Core.Implementations;

public class RandomGenerator(int? seed = null) : IRandomGenerator
{
    private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();

    public int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}