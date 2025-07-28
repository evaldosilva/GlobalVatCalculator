using Domain.VatCalculator.Interfaces.Validator;
using GlobalVatCalculator.API.Filters;
using GlobalVatCalculator.API.Validators;
using GlobalVatCalculator.API.Validators.Interface;
using Service.VatCalculator.Validator;

namespace GlobalVatCalculator.API.Extensions;

public static class RequestValidatorExtension
{
    public static IServiceCollection AddRequestValidators(this IServiceCollection services)
        => services
            .AddScoped<IPriceValidationHandler, PriceMissingValidator>()
            .AddScoped<IPriceValidationHandler, PriceMultipleInputValidator>()
            .AddScoped<IPriceValidationHandler, PriceVATTaxRateValidator>()
            .AddScoped<IPriceValidator, PriceValidator>()
            .AddScoped<IPriceRequestValidator, PriceRequestValidator>()
            .AddScoped<PriceCalculatorFilter>();
    }