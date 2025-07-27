using Domain.VatCalculator.Entities;
using Domain.VatCalculator.Interfaces.Validator;

namespace Service.VatCalculator.Validator;

public class VatRateValidator : IVatRateValidator
{
    private readonly double[] _validRates = [10d, 13d, 20d];
    public bool Validate(VatRate rate) => rate != null && _validRates.Contains(rate.Rate);
}