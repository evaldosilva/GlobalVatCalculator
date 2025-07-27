using Domain.VatCalculator.Models;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidator
{
    void Validate(Price price);
    bool IsValid();
    void SetHandler(IPriceValidationHandler priceValidationHandler);
}