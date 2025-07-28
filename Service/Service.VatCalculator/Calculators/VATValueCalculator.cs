using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class VATValueCalculator : ICalculator
{
    public async ValueTask<Price> Calculate(Price price)
    {
        Price calculatedPrice = new()
        {
            VATValue = price.VATValue,
            VATTaxRate = new(price.VATTaxRate.Rate)
        };
        calculatedPrice.NetValue = calculatedPrice.VATValue / (decimal)(calculatedPrice.VATTaxRate.Rate / 100);
        calculatedPrice.GrossValue = calculatedPrice.NetValue + calculatedPrice.VATValue;
        return await ValueTask.FromResult(calculatedPrice);
    }
}