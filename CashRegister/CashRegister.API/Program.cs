using CashRegister.Core.Implementations;
using CashRegister.Core.Interfaces;
using CashRegister.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Set up configuration to load currencies.json from the Core project
builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("Configuration/currencies.json", optional: false, reloadOnChange: true);

// Bind the configuration section to the CurrencyOptions class
builder.Services.Configure<CurrencyOptions>(builder.Configuration.GetSection("Currencies"));

// Register services
builder.Services.AddSingleton<ICurrencyRepository, CurrencyRepository>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();