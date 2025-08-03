using Domain.VatCalculator.Models;
using Domain.VatCalculator.Validation;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidator
{
    Task<Result> Validate(Price price);
}