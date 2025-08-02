using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class DefaultCalculator : ICalculator
{
    public async Task<Price> Calculate(Price price) => await Task.FromResult(price);
}