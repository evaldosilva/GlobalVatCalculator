namespace GlobalVatCalculator.API.Constants.Metadata;

internal static class PriceMetadata
{
    public const string GrossValue_request_desc = "Gets or sets the Gross value amount.";
    public const string NetValue_request_desc = "Gets or sets the Net value amount.";
    public const string VATValue_request_desc = "Gets or sets the value-added tax (VAT) amount.";
    public const string VATRate_request_desc = "The VAT rate as a percentage. Valid values are 10, 13, or 20.";
    public const string VATRate_field_desc = "VATRate";

    public const string GrossValue_result_desc = "Calculated or provided Gross value amount.";
    public const string NetValue_result_desc = "Calculated or provided Net value amount.";
    public const string VATValue_result_desc = "Calculated or provided value-added tax (VAT) amount.";
    public const string VATTaxRate_result_desc = "The VAT tax rate as a percentage.";
}