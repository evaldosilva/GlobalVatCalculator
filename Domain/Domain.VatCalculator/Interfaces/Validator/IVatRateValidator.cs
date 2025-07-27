using Domain.VatCalculator.Entities;

namespace Domain.VatCalculator.Interfaces.Validator;

public interface IVatRateValidator
{
    public bool Validate(VatRate rate);
}