using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class GrossValueCalculator : ICalculator
{
    public async Task<Price> Calculate(Price price) =>
        await Task.Run(() =>
        {
            Price calculatedPrice = new()
            {
                GrossValue = price.GrossValue,
                VATTaxRate = new(price.VATTaxRate.Rate)
            };
            calculatedPrice.NetValue = decimal.Round((decimal)(calculatedPrice.GrossValue * 100 / (decimal)(100 + calculatedPrice.VATTaxRate.Rate)), 2, MidpointRounding.ToEven);
            calculatedPrice.VATValue = decimal.Round((decimal)(calculatedPrice.GrossValue - calculatedPrice.NetValue), 2, MidpointRounding.ToEven);
            return calculatedPrice;
        });
}