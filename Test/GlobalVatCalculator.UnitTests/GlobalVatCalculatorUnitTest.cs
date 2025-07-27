using Domain.VatCalculator.Entities;
using Domain.VatCalculator.Models;
using NFluent;
using Service.VatCalculator;

namespace GlobalVatCalculator.UnitTests;

public class GlobalVatCalculatorUnitTest
{
    //     public void Should_When()
    [Fact]
    public void Should_calculate_When_set_Net_amount()
    {
        VatCalculatorService vatCalculatorService = new();

        Price price = new()
        {
            VATTaxRate = new VatRate(20),
            NetValue = 100
        };

        vatCalculatorService.CalculateVat(price);

        Check.That(price.GrossValue.Value).IsEqualTo(120);
        Check.That(price.NetValue.Value).IsEqualTo(100);
        Check.That(price.VATValue.Value).IsEqualTo(20);
    }

    [Fact]
    public void Should_calculate_When_set_Gross_amount()
    {
        VatCalculatorService vatCalculatorService = new();

        Price price = new()
        {
            VATTaxRate = new VatRate(13),
            GrossValue = 135.60m
        };

        vatCalculatorService.CalculateVat(price);

        Check.That(price.GrossValue.Value).IsEqualTo(135.60m);
        Check.That(price.NetValue.Value).IsEqualTo(120);
        Check.That(price.VATValue.Value).IsEqualTo(15.60m);
    }

    [Fact]
    public void Should_calculate_When_set_VAT_amount()
    {
        VatCalculatorService vatCalculatorService = new();

        Price price = new()
        {
            VATTaxRate = new VatRate(10),
            VATValue = 50
        };

        vatCalculatorService.CalculateVat(price);

        Check.That(price.GrossValue.Value).IsEqualTo(550);
        Check.That(price.NetValue.Value).IsEqualTo(500);
        Check.That(price.VATValue.Value).IsEqualTo(50);
    }

    [Theory, MemberData(nameof(WrongPrices))]
    public void Should_Not_calculate_When_set_multiple_invalid_input_values(Price price)
    {
        VatCalculatorService vatCalculatorService = new();
        vatCalculatorService.CalculateVat(price);

        Check.That(price.GrossValue).IsEqualTo(price.GrossValue);
        Check.That(price.NetValue).IsEqualTo(price.NetValue);
        Check.That(price.VATValue).IsEqualTo(price.VATValue);
    }

    public static TheoryData<Price> WrongPrices
    {
        get => new()
        {
            { new()
                {
                    VATTaxRate = new VatRate(20),
                    GrossValue = 0m,
                    NetValue = 0m,
                    VATValue = 0m
                }
            },
            { new()
                {
                    VATTaxRate = new VatRate(20),
                    GrossValue = null,
                    NetValue = null,
                    VATValue = null
                }
            },
            { new()
                {
                    VATTaxRate = new VatRate(20),
                    GrossValue = 120.00m,
                    NetValue = 100.00m,
                    VATValue = 20.00m
                }
            },
            { new()
                {
                    VATTaxRate = new VatRate(20),
                    GrossValue = null,
                    NetValue = 100.00m,
                    VATValue = 20.00m
                }
            },
            { new()
                {
                    VATTaxRate = new VatRate(20),
                    GrossValue = 120.00m,
                    NetValue = null,
                    VATValue = 20.00m
                }
            },
            { new()
                {
                    VATTaxRate = new VatRate(20),
                    GrossValue = 120.00m,
                    NetValue = 100.00m,
                    VATValue = null
                }
            },
            { new()
                {
                    VATTaxRate = new VatRate(99),
                    GrossValue = 120.00m,
                    NetValue = null,
                    VATValue = null
                }
            },
            { new()
                {
                    VATTaxRate = null,
                    GrossValue = 120.00m,
                    NetValue = null,
                    VATValue = null
                }
            }
        };
    }
}