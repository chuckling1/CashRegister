using CashRegister.Core.Enums;
using CashRegister.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashRegister.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChangeCalculationController(IChangeCalculatorService changeCalculatorService)
    : ControllerBase
{
    [HttpPost("bulk-calculate")]
    [Consumes("multipart/form-data")] // Specify the content type for file uploads
    public async Task<IActionResult> BulkCalculate([FromForm] IFormFile? file, [FromQuery] string currencyType = "USD")
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not provided or empty.");
        }

        if (!Enum.TryParse<CurrencyType>(currencyType, out var currencyTypeEnum))
        {
            return BadRequest("Invalid currency type.");
        }

        await using var fileStream = file.OpenReadStream();
        var resultStream = await changeCalculatorService.CalculateBulkChangeAsync(fileStream, currencyTypeEnum);
        var memoryStream = new MemoryStream();
        await resultStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "text/csv", "change-results.csv");
    }

    // [HttpPost("fruit-stand")]
    // public IActionResult FruitStand([FromBody] FruitStandTransaction transaction, [FromQuery] string currencyType = "USD")
    // {
    //     if (transaction == null || transaction.AmountPaid <= 0 || transaction.Items == null || !transaction.Items.Any())
    //     {
    //         return BadRequest("Invalid transaction data.");
    //     }
    //
    //     // try parsing currency type as enum
    //     if (!Enum.TryParse<CurrencyType>(currencyType, out var currencyTypeEnum))
    //     {
    //         return BadRequest("Invalid currency type.");
    //     }
    //     
    //     var totalAmountOwed = transaction.Items.Sum(item => item.Price * item.Quantity);
    //     var changeResult = changeCalculatorService.CalculateChange(totalAmountOwed, transaction.AmountPaid, currencyTypeEnum);
    //
    //     return Ok(changeResult);
    // }
}

// public class FruitStandTransaction
// {
//     public List<FruitItem> Items { get; set; }
//     public decimal AmountPaid { get; set; }
// }
//
// public class FruitItem
// {
//     public string Name { get; set; }
//     public decimal Price { get; set; }
//     public int Quantity { get; set; }
// }