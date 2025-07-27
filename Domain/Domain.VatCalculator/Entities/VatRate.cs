namespace Domain.VatCalculator.Entities;

public class VatRate(double rate)
{
    public double Rate { get; } = rate;
}