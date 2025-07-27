using GlobalVatCalculator.API.Constants.Metadata;
using GlobalVatCalculator.API.Validators.Interface;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GlobalVatCalculator.API.Requests;

public class PriceRequest : IValidatableObject
{
    [JsonPropertyName(PriceMetadata.VATRate_field_desc)]
    [Description(PriceMetadata.VATRate_request_desc)]
    public int VATRate { get; set; }

    [Description(PriceMetadata.GrossValue_request_desc)]
    public decimal? GrossValue { get; set; }

    [Description(PriceMetadata.NetValue_request_desc)]
    public decimal? NetValue { get; set; }

    [Description()]
    public decimal? VATValue { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => validationContext.GetRequiredService<IPriceRequestValidator>()
                            .Validate(this);
}