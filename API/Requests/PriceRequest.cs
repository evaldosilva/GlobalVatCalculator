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

    [Description("Gets or sets the Gross value amount.")]
    public decimal? GrossValue { get; set; }

    [Description("Gets or sets the Net value amount.")]
    public decimal? NetValue { get; set; }

    [Description("Gets or sets the value-added tax (VAT) amount.")]
    public decimal? VATValue { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => validationContext.GetRequiredService<IPriceRequestValidator>()
                            .Validate(this);
}