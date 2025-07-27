using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Validator
{
    public class PriceMultipleInputValidator : PriceValidationHandler
    {
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
}