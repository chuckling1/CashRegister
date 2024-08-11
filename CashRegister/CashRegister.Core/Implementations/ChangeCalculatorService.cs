using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
namespace CashRegister.Core.Implementations;

public class ChangeCalculatorService(
    IChangeCalculator defaultChangeCalculator,
    IEnumerable<IOptionalChangeCalculator>? optionalChangeCalculators = null) : IChangeCalculatorService
{
    private readonly IEnumerable<IOptionalChangeCalculator> _optionalChangeCalculators = optionalChangeCalculators ?? [];
    
    public string CalculateChange(decimal amountOwed, decimal amountPaid, CurrencyType currencyType)
    {
        foreach (var calculator in _optionalChangeCalculators)
        {
            if (calculator.DoesApplyToTransaction(amountOwed, amountPaid))
            {
                return calculator
                    .Calculate(amountOwed, amountPaid, currencyType)
                    .AsChangeSummaryString();
            }
        }

        // Fallback to the default calculator if no optional calculators apply
        return defaultChangeCalculator
            .Calculate(amountOwed, amountPaid, currencyType)
            .AsChangeSummaryString();
    }
    
    public async Task<Stream> CalculateBulkChangeAsync(Stream fileStream, CurrencyType currencyType)
    {
        var outputStream = new MemoryStream();
        using var reader = new StreamReader(fileStream);
        await using var writer = new StreamWriter(outputStream);

        while (await reader.ReadLineAsync() is { } line)
        {
            var parts = line.Split(',');
            if (parts.Length != 2 
                || !decimal.TryParse(parts[0], out var amountOwed)
                || !decimal.TryParse(parts[1], out var amountPaid))
            {
                throw new InvalidDataException("Invalid file format. Each line should contain two decimal values separated by a comma.");
            }

            var result = CalculateChange(amountOwed, amountPaid, currencyType);
            await writer.WriteLineAsync($"{line},{result}");
        }

        await writer.FlushAsync();
        outputStream.Position = 0;

        return outputStream;
    }
}