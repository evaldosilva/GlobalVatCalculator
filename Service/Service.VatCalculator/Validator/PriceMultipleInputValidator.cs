using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Domain.VatCalculator.Validation;

namespace Service.VatCalculator.Validator;

public class PriceMultipleInputValidator : PriceValidationHandler
{
    public override PriceValidationHandlerType Type => PriceValidationHandlerType.PriceMultipleInputValidator;
    protected override Result DoHandle(Price price)
    {
        if (price.NetValue.HasValue && price.GrossValue.HasValue)
            return Result.Failure(ValidationErrors.ErrorPricesMultipleInput, [nameof(price.NetValue), nameof(price.GrossValue)]);
        else if (price.NetValue.HasValue && price.VATValue.HasValue)
            return Result.Failure(ValidationErrors.ErrorPricesMultipleInput, [nameof(price.NetValue), nameof(price.VATValue)]);
        else if (price.GrossValue.HasValue && price.VATValue.HasValue)
            return Result.Failure(ValidationErrors.ErrorPricesMultipleInput, [nameof(price.GrossValue), nameof(price.VATValue)]);
        else
            return Result.Success();
    }
}