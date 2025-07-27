using GlobalVatCalculator.API.Constants.Metadata;
using System.ComponentModel;

namespace GlobalVatCalculator.API.Results;

public class PriceResult
{
    [Description(PriceMetadata.VATTaxRate_result_desc)]
    public double VATTaxRate { get; set; }

    [Description(PriceMetadata.NetValue_result_desc)]
    public decimal? NetValue { get; set; }

    [Description(PriceMetadata.VATValue_result_desc)]
    public decimal? VATValue { get; set; }

    [Description(PriceMetadata.GrossValue_result_desc)]
    public decimal? GrossValue { get; set; }
}