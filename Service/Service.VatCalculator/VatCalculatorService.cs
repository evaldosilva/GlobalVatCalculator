using Domain.VatCalculator.Interfaces.Service;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator;

public class VatCalculatorService : IVatCalculator
{
    public Price CalculateVat(Price price)
    {
        if (price.NetValue.HasValue && price.NetValue > 0)
        {
            price.NetValue = price.NetValue;
            price.VATValue = price.NetValue * (decimal)(price.VATTaxRate.Rate / 100);
            price.GrossValue = price.NetValue + price.VATValue;
        }
        else if (price.GrossValue.HasValue && price.GrossValue > 0)
        {
            price.GrossValue = price.GrossValue;
            price.NetValue = (price.GrossValue * 100) / (decimal)(100 + price.VATTaxRate.Rate);
            price.VATValue = price.GrossValue - price.NetValue;

        }
        else if (price.VATValue.HasValue && price.VATValue > 0)
        {
            price.VATValue = price.VATValue;
            price.NetValue = price.VATValue / (decimal)(price.VATTaxRate.Rate / 100);
            price.GrossValue = price.NetValue + price.VATValue;
        }
        else
            throw new NotImplementedException();

        return price;
    }
}