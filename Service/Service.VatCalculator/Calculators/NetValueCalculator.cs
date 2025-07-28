using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class NetValueCalculator : ICalculator
{
    public async ValueTask<Price> Calculate(Price price)
    {
        Price calculatedPrice = new()
        {
            NetValue = price.NetValue,
            VATTaxRate = new(price.VATTaxRate.Rate)
        };
        calculatedPrice.VATValue = calculatedPrice.NetValue * (decimal)(calculatedPrice.VATTaxRate.Rate / 100);
        calculatedPrice.GrossValue = calculatedPrice.NetValue + calculatedPrice.VATValue;
        return await ValueTask.FromResult(calculatedPrice);
    }
}