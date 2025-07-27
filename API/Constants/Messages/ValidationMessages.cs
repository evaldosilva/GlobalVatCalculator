namespace GlobalVatCalculator.API.Constants.Messages;

internal static class ValidationMessages
{
    public const string VatRateInvalid = "VAT rate is required and must be the positive number 10, 13 or 20.";
    public const string PricesMissingOrInvalid = "Price values (VAT amount, Net or Gross) are missing or have an invalid values. One of them is required and must be positive.";
    public const string PricesMultipleInput = "Just one value (VAT amount, Net or Gross) must be provided.";
}