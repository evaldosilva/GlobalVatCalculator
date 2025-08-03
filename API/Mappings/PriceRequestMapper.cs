using Domain.VatCalculator.Entities;
using Domain.VatCalculator.Models;
using GlobalVatCalculator.API.Requests;

namespace GlobalVatCalculator.API.Mappings;

public static class PriceRequestMapper
{
    public static Price MapPriceRequestToPrice(PriceRequest priceRequest)
    {
        return new Price
        {
            NetValue = priceRequest?.NetValue,
            GrossValue = priceRequest?.GrossValue,
            VATValue = priceRequest?.VATValue,
            VATTaxRate = new VatRate(priceRequest!.VATRate)
        };
    }
}