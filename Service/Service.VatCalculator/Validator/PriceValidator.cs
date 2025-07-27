using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Validator;

public class PriceValidator : IPriceValidator
{
    public bool Validate(Price price)
    {
        if(price == null)
            return false;

        // Some must have value
        if (price.NetValue.HasValue || price.GrossValue.HasValue || price.VATValue.HasValue)
        {
            // But just one must have value
            if (price.NetValue.HasValue && price.GrossValue.HasValue)
                return false;
            else if (price.NetValue.HasValue && price.VATValue.HasValue)
                return false;
            else if (price.GrossValue.HasValue && price.VATValue.HasValue)
                return false;
            else
                return true;
        }
        return false;
    }
}