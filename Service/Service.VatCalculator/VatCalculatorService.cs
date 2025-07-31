using Domain.VatCalculator.Interfaces.Service;
using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Service.VatCalculator.Calculators;
using Service.VatCalculator.Validator;

namespace Service.VatCalculator;

public class VatCalculatorService(IEnumerable<IPriceValidationHandler> priceValidationHandlers) : IVatCalculator
{
    private readonly IEnumerable<IPriceValidationHandler> _priceValidationHandlers = priceValidationHandlers;

    public async ValueTask<Price> CalculateVat(Price price)
    {
        if (ValidatePrice(price))
            return await CalculatorFactory.GetCalculator(price.CalculationType).Calculate(price);
        else
            return await ValueTask.FromResult(price);
    }

    private bool ValidatePrice(Price price)
    {
        IPriceValidationHandler validators = _priceValidationHandlers
            .First(h => h.Type == PriceValidationHandlerType.PriceMissingValidator);

        validators
            .SetNext(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceMultipleInputValidator))
            .SetNext(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceVATTaxRateValidator));

        PriceValidator priceValidation = new(validators);
        return priceValidation.Validate(price);
    }
}