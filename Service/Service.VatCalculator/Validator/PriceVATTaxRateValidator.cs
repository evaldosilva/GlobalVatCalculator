using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;

namespace Service.VatCalculator.Validator;

public class PriceVATTaxRateValidator : PriceValidationHandler
{
    public override PriceValidationHandlerType Type => PriceValidationHandlerType.PriceVATTaxRateValidator;
    private readonly double[] _validRates = [10d, 13d, 20d];
    protected override bool DoHandle(Price price)
        => price?.VATTaxRate != null && _validRates.Contains(price.VATTaxRate.Rate);
}