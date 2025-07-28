using Domain.VatCalculator.Models;

namespace Domain.VatCalculator.Interfaces.Calculator;

public interface ICalculator
{
    /// <summary>
    /// Calculates the price amounts based on the strategy type.
    /// </summary>
    /// <param name="price">Price to be calculated</param>
    /// <returns>A new calculated price</returns>
    ValueTask<Price> Calculate(Price price);
}