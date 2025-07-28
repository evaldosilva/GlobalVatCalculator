using GlobalVatCalculator.API.Requests;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators.Interface;

public interface IPriceRequestValidator
{
    /// <summary>
    /// Validates the PriceRequest values.
    /// </summary>
    /// <param name="priceRequest">The PriceRequest payload</param>
    /// <returns>An error list of failed validations</returns>
    ValueTask<IEnumerable<ValidationResult>> Validate(PriceRequest priceRequest);
}