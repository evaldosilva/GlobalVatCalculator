using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class GrossValueCalculator : ICalculator
{
    public async ValueTask<Price> Calculate(Price price)
    {
        Price calculatedPrice = new()
        {
            GrossValue = price.GrossValue,
            VATTaxRate = new(price.VATTaxRate.Rate)
        };
        calculatedPrice.NetValue = (calculatedPrice.GrossValue * 100) / (decimal)(100 + calculatedPrice.VATTaxRate.Rate);
        calculatedPrice.VATValue = calculatedPrice.GrossValue - calculatedPrice.NetValue;
        return await ValueTask.FromResult(calculatedPrice);
    }
}