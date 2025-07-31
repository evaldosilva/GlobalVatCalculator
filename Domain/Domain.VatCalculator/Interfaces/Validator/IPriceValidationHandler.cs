using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IPriceValidationHandler
{
    PriceValidationHandlerType Type { get; }
    bool Handle(Price price);
    IPriceValidationHandler SetNext(IPriceValidationHandler nextHandler);
}