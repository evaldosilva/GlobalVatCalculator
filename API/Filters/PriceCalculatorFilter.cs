using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.VatCalculator.Validator;

namespace GlobalVatCalculator.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class PriceCalculatorFilter : Attribute, IAsyncActionFilter
{
    private readonly PriceRequestValidator _priceRequestValidator;
    private readonly ILogger<PriceCalculatorFilter> _logger;

    public PriceCalculatorFilter(ILogger<PriceCalculatorFilter> logger)
    {
        _priceRequestValidator = new PriceRequestValidator(new PriceValidator(new PriceMissingValidator()));
        _logger = logger;
    }

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