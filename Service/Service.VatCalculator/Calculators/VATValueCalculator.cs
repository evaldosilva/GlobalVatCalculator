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
        calculatedPrice.NetValue = decimal.Round((decimal)(calculatedPrice.VATValue / (decimal)(calculatedPrice.VATTaxRate.Rate / 100)), 2, MidpointRounding.ToEven);
        calculatedPrice.GrossValue = decimal.Round((decimal)(calculatedPrice.NetValue + calculatedPrice.VATValue), 2, MidpointRounding.ToEven);
        return await ValueTask.FromResult(calculatedPrice);
    }
}