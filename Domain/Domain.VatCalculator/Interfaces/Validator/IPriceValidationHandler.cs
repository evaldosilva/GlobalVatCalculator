using Domain.VatCalculator.Models;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidationHandler
{
    public bool Handle(Price price);
    public bool IsValid { get; }
}