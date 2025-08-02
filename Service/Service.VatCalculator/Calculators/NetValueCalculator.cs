using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class NetValueCalculator : ICalculator
{
    public async Task<Price> Calculate(Price price) => 
        await Task.Run(() =>
        {
            Price calculatedPrice = new()
            {
                NetValue = price.NetValue,
                VATTaxRate = new(price.VATTaxRate.Rate)
            };
            calculatedPrice.VATValue = decimal.Round((decimal)(calculatedPrice.NetValue * (decimal)(calculatedPrice.VATTaxRate.Rate / 100)), 2, MidpointRounding.ToEven);
            calculatedPrice.GrossValue = decimal.Round((decimal)(calculatedPrice.NetValue + calculatedPrice.VATValue), 2, MidpointRounding.ToEven);
            return calculatedPrice;
        });
}