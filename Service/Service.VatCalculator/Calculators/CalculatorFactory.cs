using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Types;

namespace Service.VatCalculator.Calculators;

public static class CalculatorFactory
{
    private static readonly ICalculator _defaultCalculator = new DefaultCalculator();
    private static readonly Dictionary<CalculationType, ICalculator> _calculators = new()
    {
        { CalculationType.NetValueCalculation, new NetValueCalculator() },
        { CalculationType.GrossValueCalculation, new GrossValueCalculator() },
        { CalculationType.VATValueCalculation, new VATValueCalculator() }
    };

    public static ICalculator GetCalculator(CalculationType calculationType = CalculationType.Default)
        => _calculators.GetValueOrDefault(calculationType, _defaultCalculator);
}