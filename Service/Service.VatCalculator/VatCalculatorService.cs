using Domain.VatCalculator.Interfaces.Service;
using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Service.VatCalculator.Validator;

namespace Service.VatCalculator;

public class VatCalculatorService(IEnumerable<IPriceValidationHandler> priceValidationHandlers) : IVatCalculator
{
    private readonly IEnumerable<IPriceValidationHandler> _priceValidationHandlers = priceValidationHandlers;

    public async ValueTask<Price> CalculateVat(Price price)
    {
        var validators = _priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceMissingValidator)
            .SetNext(_priceValidationHandlers
                .First(h => h.Type == PriceValidationHandlerType.PriceMultipleInputValidator))
            .SetNext(_priceValidationHandlers
                .First(h => h.Type == PriceValidationHandlerType.PriceVATTaxRateValidator));

        PriceValidator priceValidation = new(validators);
        priceValidation.Validate(price);

        if (priceValidation.IsValid())
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

        return await ValueTask.FromResult(price);
    }
}