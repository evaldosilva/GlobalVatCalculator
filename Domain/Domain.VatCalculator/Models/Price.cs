using Domain.VatCalculator.Entities;
using Domain.VatCalculator.Types;

namespace Domain.VatCalculator.Models;

public class Price
{
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? VATValue { get; set; }
    public VatRate? VATTaxRate { get; init; }

    public CalculationType CalculationType
    {
        get
        {
            if (ShouldCalculateByNet(this))
                return CalculationType.NetValueCalculation;
            else if (ShouldCalculateByGross(this))
                return CalculationType.GrossValueCalculation;
            else if (ShouldCalculateByVat(this))
                return CalculationType.VATValueCalculation;
            else
                return CalculationType.Default;
        }
    }

    private static bool ShouldCalculateByGross(Price price) => price.GrossValue.HasValue && price.GrossValue > 0 && !price.VATValue.HasValue && !price.NetValue.HasValue;
    private static bool ShouldCalculateByVat(Price price) => price.VATValue.HasValue && price.VATValue > 0 && !price.GrossValue.HasValue && !price.NetValue.HasValue;
    private static bool ShouldCalculateByNet(Price price) => price.NetValue.HasValue && price.NetValue > 0 && !price.VATValue.HasValue && !price.GrossValue.HasValue;
}