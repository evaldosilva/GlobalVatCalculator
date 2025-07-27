using Domain.VatCalculator.Interfaces.Service;
using Service.VatCalculator;

namespace GlobalVatCalculator.API.Extensions;

public static class BusinessServiceExtension
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IVatCalculator, VatCalculatorService>();
    }
}