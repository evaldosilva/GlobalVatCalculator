using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Validator;

public abstract class PriceValidationHandler : IPriceValidationHandler
{
    private PriceValidationHandler? _nextHandler;
    public bool IsValid { get; private set; } = false;

    public PriceValidationHandler SetNext(PriceValidationHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return _nextHandler;
    }

    public bool Handle(Price price)
    {
        if (!DoHandle(price))
        {
            IsValid = false;
            return IsValid;
        }
        else
        {
            if (_nextHandler != null)
            {
                IsValid = _nextHandler.Handle(price);
                return IsValid;
            }
            else
            {
                IsValid = true;
                return IsValid;
            }
        }
    }

    protected abstract bool DoHandle(Price price);
}