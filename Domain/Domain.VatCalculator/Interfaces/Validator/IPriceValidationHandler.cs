using Domain.VatCalculator.Models;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidationHandler
{
    bool Handle(Price price);
    bool IsValid { get; }
}