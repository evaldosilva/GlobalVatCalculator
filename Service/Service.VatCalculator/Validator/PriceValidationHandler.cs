using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Domain.VatCalculator.Validation;

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

    public async Task<Result> Handle(Price price)
    {
        Result result = DoHandle(price);
        if (result.IsSuccess)
        {
            if (_nextHandler != null)
                return await _nextHandler.Handle(price);
            else
                return await Task.FromResult(Result.Success());
        }
        else
            return await Task.FromResult(result);
    }

    protected abstract Result DoHandle(Price price);
}