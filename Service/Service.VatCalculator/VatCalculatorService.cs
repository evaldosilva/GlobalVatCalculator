using Domain.VatCalculator.Interfaces.Service;
using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Domain.VatCalculator.Validation;
using Service.VatCalculator.Calculators;
using Service.VatCalculator.Validator;

namespace Service.VatCalculator;

public class VatCalculatorService(IEnumerable<IPriceValidationHandler> priceValidationHandlers) : IVatCalculator
{
    private readonly IEnumerable<IPriceValidationHandler> _priceValidationHandlers = priceValidationHandlers;

    public async Task<Price> CalculateVat(Price price)
    {
        if ((await ValidatePrice(price)).IsSuccess)
            return await CalculatorFactory.GetCalculator(price).Calculate(price);
        else
            return await Task.FromResult(price);
    }


    public async Task<Result> ValidatePrice(Price price)
    {
        IPriceValidationHandler validators = _priceValidationHandlers
            .First(h => h.Type == PriceValidationHandlerType.PriceMissingValidator);

        validators
            .SetNext(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceMultipleInputValidator))
            .SetNext(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceVATTaxRateValidator));

        PriceValidator priceValidation = new(validators);
        return await priceValidation.Validate(price);
    }
}