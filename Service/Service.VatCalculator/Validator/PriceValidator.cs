using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Validator;

public class PriceValidator(IPriceValidationHandler priceValidationHandler) : IPriceValidator
{
    private IPriceValidationHandler _priceValidationHandler = priceValidationHandler;

    public bool IsValid() => _priceValidationHandler.IsValid;

    public void Validate(Price price)
    {
        _priceValidationHandler.Handle(price);
    }

    public void SetHandler(IPriceValidationHandler priceValidationHandler)
    {
        _priceValidationHandler = priceValidationHandler;
    }
}