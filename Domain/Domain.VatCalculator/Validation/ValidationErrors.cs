namespace Domain.VatCalculator.Validation;

public static class ValidationErrors
{
    public static readonly Error ErrorVatRateInvalid = new("VAT rate required", "VAT rate is required and must be the positive number 10, 13 or 20.");
    public static readonly Error ErrorPricesMissingOrInvalid = new("Invalid price", "Price values (VAT amount, Net or Gross) are missing or have an invalid values. One of them is required and must be positive.");
    public static readonly Error ErrorPricesMultipleInput = new("Multiple values inputed", "Just one value (VAT amount, Net or Gross) must be provided.");
    public static readonly Error ErrorInvalidInput = new ("Invalid input", "Provided values are not valid");
}