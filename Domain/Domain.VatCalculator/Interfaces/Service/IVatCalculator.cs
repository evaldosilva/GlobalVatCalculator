using Domain.VatCalculator.Models;
using Domain.VatCalculator.Validation;

namespace Domain.VatCalculator.Interfaces.Service;

public interface IVatCalculator
{
    /// <summary>
    /// Calculates the Net, Gross and VAT amounts for purchases based on the given purchase data.
    /// </summary>
    /// <param name="price">The price of the item.</param>
    /// <param name="vatRate">The VAT rate as a percentage.</param>
    /// <returns>The calculated Net, Gross and VAT amounts.</returns>
    Task<Price> CalculateVat(Price price);

    /// <summary>
    /// Validate price input
    /// </summary>
    /// <param name="price">Given price</param>
    /// <returns>A validation result for the given price</returns>
    Task<Result> ValidatePrice(Price price);
}