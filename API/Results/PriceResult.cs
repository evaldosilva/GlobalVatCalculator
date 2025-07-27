using System.ComponentModel;

namespace GlobalVatCalculator.API.Results;

public class PriceResult
{
    [Description("The VAT tax rate as a percentage.")]
    public double VATTaxRate { get; set; }

    [Description("Calculated or provided Net value amount.")]
    public decimal? NetValue { get; set; }

    [Description("Calculated or provided value-added tax (VAT) amount.")]
    public decimal? VATValue { get; set; }

    [Description("Calculated or provided Gross value amount.")]
    public decimal? GrossValue { get; set; }
}