using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Validation;

namespace Service.VatCalculator.Validator;

public class PriceValidator(IPriceValidationHandler priceValidationHandler) : IPriceValidator
{
    private readonly IPriceValidationHandler _priceValidationHandler = priceValidationHandler;
    public async Task<Result> Validate(Price price) => await _priceValidationHandler.Handle(price);
}