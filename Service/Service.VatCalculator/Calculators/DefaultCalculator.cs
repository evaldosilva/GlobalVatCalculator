using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public class DefaultCalculator : ICalculator
{
    public async ValueTask<Price> Calculate(Price price) => await ValueTask.FromResult(price);
}