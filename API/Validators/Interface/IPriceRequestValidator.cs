using GlobalVatCalculator.API.Requests;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators.Interface;

public interface IPriceRequestValidator
{
    ValueTask<IEnumerable<ValidationResult>> Validate(PriceRequest priceRequest);
}