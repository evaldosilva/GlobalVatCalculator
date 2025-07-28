using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Validators.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GlobalVatCalculator.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class PriceCalculatorFilter(ILogger<PriceCalculatorFilter> logger, IPriceRequestValidator priceRequestValidator) : Attribute, IAsyncActionFilter
{
    private readonly IPriceRequestValidator _priceRequestValidator = priceRequestValidator;
    private readonly ILogger<PriceCalculatorFilter> _logger = logger;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var priceRequest = context?.ActionArguments["priceRequest"] as PriceRequest;
        var validationResults = await _priceRequestValidator.Validate(priceRequest);

        _logger.LogWarning("PriceRequest {@priceRequest} validation results: {ValidationResults}", priceRequest, validationResults);

        if (validationResults.Any())
            context.Result = new BadRequestObjectResult(validationResults);
        else
            await next();
    }
}