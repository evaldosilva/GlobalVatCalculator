using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Domain.VatCalculator.Validation;

namespace Service.VatCalculator.Validator;

public class PriceMissingValidator : PriceValidationHandler
{
    public override PriceValidationHandlerType Type => PriceValidationHandlerType.PriceMissingValidator;
    protected override Result DoHandle(Price price)
    {
        if ((!price.NetValue.HasValue || !price.GrossValue.HasValue || !price.VATValue.HasValue)
            && (price.NetValue > 0 || price.GrossValue > 0 || price.VATValue > 0))
            return Result.Success();
        else
            return Result.Failure(ValidationErrors.ErrorPricesMissingOrInvalid,
                    [nameof(price.GrossValue), nameof(price.VATValue), nameof(price.NetValue)]);
    }
}