namespace Domain.VatCalculator.Types;

public enum PriceValidationHandlerType
{
    None = 0,
    PriceMissingValidator = 1,
    PriceMultipleInputValidator = 2,
    PriceVATTaxRateValidator = 3
}