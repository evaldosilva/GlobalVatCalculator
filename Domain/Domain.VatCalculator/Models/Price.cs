using Domain.VatCalculator.Entities;

namespace Domain.VatCalculator.Models;

public class Price
{
    public decimal? GrossValue { get; set; }
    public decimal? NetValue { get; set; }
    public decimal? VATValue { get; set; }
    public VatRate? VATTaxRate { get; init; }
}