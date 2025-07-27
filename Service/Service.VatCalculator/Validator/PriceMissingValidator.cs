using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Validator;

public class PriceMissingValidator : PriceValidationHandler
{
    protected override bool DoHandle(Price price) 
        => (!price.NetValue.HasValue || !price.GrossValue.HasValue || !price.VATValue.HasValue)
            && (price.NetValue > 0 || price.GrossValue > 0 || price.VATValue > 0);
}