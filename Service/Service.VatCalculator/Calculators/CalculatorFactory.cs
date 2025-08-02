using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Models;

namespace Service.VatCalculator.Calculators;

public static class CalculatorFactory
{
    public static ICalculator GetCalculator(Price price)
    {
        if (ShouldCalculateByNet(price))
            return new NetValueCalculator();
        else if (ShouldCalculateByGross(price))
            return new GrossValueCalculator();
        else if (ShouldCalculateByVat(price))
            return new VATValueCalculator();
        else
            return new DefaultCalculator();
    }

    private static bool ShouldCalculateByGross(Price price) =>
        price.GrossValue.HasValue && price.GrossValue > 0 && !price.VATValue.HasValue && !price.NetValue.HasValue;
    private static bool ShouldCalculateByVat(Price price) =>
        price.VATValue.HasValue && price.VATValue > 0 && !price.GrossValue.HasValue && !price.NetValue.HasValue;
    private static bool ShouldCalculateByNet(Price price) =>
        price.NetValue.HasValue && price.NetValue > 0 && !price.VATValue.HasValue && !price.GrossValue.HasValue;
}