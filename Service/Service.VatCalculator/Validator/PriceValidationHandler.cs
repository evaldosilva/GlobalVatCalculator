using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;

namespace Service.VatCalculator.Validator;

public abstract class PriceValidationHandler : IPriceValidationHandler
{
    private IPriceValidationHandler? _nextHandler;
    public bool IsValid { get; private set; } = false;

    public virtual PriceValidationHandlerType Type => PriceValidationHandlerType.None;

    public IPriceValidationHandler SetNext(IPriceValidationHandler nextHandler)
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