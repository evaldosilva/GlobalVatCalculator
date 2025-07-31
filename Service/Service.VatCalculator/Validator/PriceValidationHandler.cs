using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;

namespace Service.VatCalculator.Validator;

public abstract class PriceValidationHandler : IPriceValidationHandler
{
    private IPriceValidationHandler? _nextHandler;

    public virtual PriceValidationHandlerType Type => PriceValidationHandlerType.None;

    public IPriceValidationHandler SetNext(IPriceValidationHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return _nextHandler;
    }

    public bool Handle(Price price)
    {
        if (DoHandle(price))
        {
            if (_nextHandler != null)
                return _nextHandler.Handle(price);
            else
                return true;
        }
        else
            return false;
    }

    protected abstract bool DoHandle(Price price);
}