using Domain.VatCalculator.Entities;
using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using NFluent;
using Service.VatCalculator;
using Service.VatCalculator.Validator;

namespace GlobalVatCalculator.UnitTests;

public class GlobalVatCalculatorUnitTest
{
    [Fact]
    public async Task Should_calculate_When_set_Net_amount()
    {
        VatCalculatorService vatCalculatorService = new(GetPriceValidationHandlers());

        Price price = new()
        {
            VATTaxRate = new VatRate(20),
            NetValue = 100
        };

        var calculatedPrice = await vatCalculatorService.CalculateVat(price);

        Check.That(calculatedPrice?.GrossValue).IsEqualTo(120);
        Check.That(calculatedPrice?.NetValue).IsEqualTo(100);
        Check.That(calculatedPrice?.VATValue).IsEqualTo(20);
    }

    [Fact]
    public async Task Should_calculate_When_set_Gross_amount()
    {
        VatCalculatorService vatCalculatorService = new(GetPriceValidationHandlers());

        Price price = new()
        {
            VATTaxRate = new VatRate(13),
            GrossValue = 135.60m
        };

        var calculatedPrice = await vatCalculatorService.CalculateVat(price);

        Check.That(calculatedPrice?.GrossValue).IsEqualTo(135.60m);
        Check.That(calculatedPrice?.NetValue).IsEqualTo(120);
        Check.That(calculatedPrice?.VATValue).IsEqualTo(15.60m);
    }

    [Fact]
    public async Task Should_calculate_When_set_VAT_amount()
    {
        VatCalculatorService vatCalculatorService = new(GetPriceValidationHandlers());

        Price price = new()
        {
            VATTaxRate = new VatRate(10),
            VATValue = 50
        };

        var calculatedPrice = await vatCalculatorService.CalculateVat(price);

        Check.That(calculatedPrice?.GrossValue).IsEqualTo(550);
        Check.That(calculatedPrice?.NetValue).IsEqualTo(500);
        Check.That(calculatedPrice?.VATValue).IsEqualTo(50);
    }

    [Theory, MemberData(nameof(WrongPrices))]
    public async Task Should_Not_calculate_When_set_multiple_invalid_input_values(Price price)
    {
        VatCalculatorService vatCalculatorService = new(GetPriceValidationHandlers());
        var calculatedPrice = await vatCalculatorService.CalculateVat(price);

        Check.That(calculatedPrice?.GrossValue).IsEqualTo(price.GrossValue);
        Check.That(calculatedPrice?.NetValue).IsEqualTo(price.NetValue);
        Check.That(calculatedPrice?.VATValue).IsEqualTo(price.VATValue);
    }

    private static IEnumerable<IPriceValidationHandler> GetPriceValidationHandlers()
    {
        return
        [
            new PriceMissingValidator(),
            new PriceMultipleInputValidator(),
            new PriceVATTaxRateValidator()
        ];
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
            },
            { new()
                {
                    VATTaxRate = new VatRate(0),
                    GrossValue = 120.00m,
                    NetValue = null,
                    VATValue = null
                }
            }
        };
    }
}