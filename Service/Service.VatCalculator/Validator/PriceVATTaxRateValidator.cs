using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Domain.VatCalculator.Validation;

namespace Service.VatCalculator.Validator;

public class PriceVATTaxRateValidator : PriceValidationHandler
{
    public override PriceValidationHandlerType Type => PriceValidationHandlerType.PriceVATTaxRateValidator;
    private readonly double[] _validRates = [10d, 13d, 20d];
    protected override Result DoHandle(Price price)
    {
        if (price?.VATTaxRate != null && _validRates.Contains(price.VATTaxRate.Rate))
            return Result.Success();
        else
            return Result.Failure(ValidationErrors.ErrorVatRateInvalid, [nameof(price.VATTaxRate)]);
    }
}