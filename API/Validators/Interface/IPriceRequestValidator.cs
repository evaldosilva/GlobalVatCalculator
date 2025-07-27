using GlobalVatCalculator.API.Requests;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators.Interface;

public interface IPriceRequestValidator
{
    public IEnumerable<ValidationResult> Validate(PriceRequest priceRequest);
}