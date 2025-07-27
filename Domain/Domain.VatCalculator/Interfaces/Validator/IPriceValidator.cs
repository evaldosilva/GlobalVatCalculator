using Domain.VatCalculator.Models;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidator
{
    bool Validate(Price price);
}