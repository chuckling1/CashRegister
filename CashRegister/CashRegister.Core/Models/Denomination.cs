namespace CashRegister.Core.Models;

public class Denomination(string name, string pluralName, decimal value)
{
    public string Name { get; } = name;
    public string PluralName { get; set;  } = pluralName;
    public decimal Value { get; set; } = value;

    // Override Equals and GetHashCode for proper dictionary key behavior
    public override bool Equals(object? obj)
    {
        if (obj is Denomination other)
        {
            return Name == other.Name 
                   && PluralName == other.PluralName 
                   && Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, PluralName, Value);
    }
}
