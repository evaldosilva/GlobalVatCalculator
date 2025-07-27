using Domain.VatCalculator.Models;
using GlobalVatCalculator.API.Results;

namespace GlobalVatCalculator.API.Mappings;

public static class PriceResultMapper
{
    public static PriceResult MapPriceToPriceResult(Price price)
    {
        return new PriceResult
        {
            NetValue = price.NetValue,
            GrossValue = price.GrossValue,
            VATValue = price.VATValue,
            VATTaxRate = price?.VATTaxRate?.Rate ?? 0
        };
    }
}