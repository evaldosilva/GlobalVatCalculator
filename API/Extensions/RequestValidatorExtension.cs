using Domain.VatCalculator.Interfaces.Validator;
using GlobalVatCalculator.API.Filters;
using GlobalVatCalculator.API.Validators;
using GlobalVatCalculator.API.Validators.Interface;
using Service.VatCalculator.Validator;

namespace GlobalVatCalculator.API.Extensions;

public static class RequestValidatorExtension
{
    public static IServiceCollection AddRequestValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<PriceCalculatorFilter>()
            .AddScoped<IPriceValidationHandler, PriceMissingValidator>()
            .AddScoped<IPriceValidator, PriceValidator>()
            .AddScoped<IPriceRequestValidator, PriceRequestValidator>();
    }
}