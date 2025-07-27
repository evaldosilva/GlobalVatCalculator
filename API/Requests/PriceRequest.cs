using GlobalVatCalculator.API.Constants.Metadata;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GlobalVatCalculator.API.Requests;

public class PriceRequest
{
    [JsonPropertyName(PriceMetadata.VATRate_field_desc)]
    [Description(PriceMetadata.VATRate_request_desc)]
    public int VATRate { get; set; }

    [Description(PriceMetadata.GrossValue_request_desc)]
    public decimal? GrossValue { get; set; }

    [Description(PriceMetadata.NetValue_request_desc)]
    public decimal? NetValue { get; set; }

    [Description(PriceMetadata.VATValue_result_desc)]
    public decimal? VATValue { get; set; }
}