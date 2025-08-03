using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Domain.VatCalculator.Validation;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidationHandler
{
    PriceValidationHandlerType Type { get; }
    Task<Result> Handle(Price price);
    IPriceValidationHandler SetNext(IPriceValidationHandler nextHandler);
}