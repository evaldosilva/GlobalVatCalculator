using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;

namespace Service.VatCalculator.Validator;

public class PriceMultipleInputValidator : PriceValidationHandler
{
    public override PriceValidationHandlerType Type => PriceValidationHandlerType.PriceMultipleInputValidator;
    protected override bool DoHandle(Price price)
    {
        if (price.NetValue.HasValue && price.GrossValue.HasValue)
            return false;
        else if (price.NetValue.HasValue && price.VATValue.HasValue)
            return false;
        else if (price.GrossValue.HasValue && price.VATValue.HasValue)
            return false;
        else
            return true;
    }
}