using Domain.VatCalculator.Interfaces.Service;
using GlobalVatCalculator.API.Constants.Routes;
using GlobalVatCalculator.API.Mappings;
using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Results;
using Microsoft.AspNetCore.Mvc;

namespace GlobalVatCalculator.API.Controllers;

[ApiController]
[Route(RouteDefinitions.V1.Base)]
public class VatCalculatorController(IVatCalculator vatCalculator) : ControllerBase
{
    private readonly IVatCalculator _vatCalculator = vatCalculator;

    [HttpPost(RouteDefinitions.V1.PriceCalculatorEndpoint)]
    [ProducesResponseType(typeof(PriceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces(RouteDefinitions.ApplicationJson)]
    [Consumes(RouteDefinitions.ApplicationJson)]
    [EndpointDescription(RouteDefinitions.V1.PriceCalculatorEndpointDesc)]
    public async ValueTask<IActionResult> PriceCalculator([FromBody] PriceRequest priceRequest)
    {
        var price = PriceRequestMapper.MapPriceRequestToPrice(priceRequest);
        var calculatedPrice = await _vatCalculator.CalculateVat(price);
        return Ok(PriceResultMapper.MapPriceToPriceResult(calculatedPrice));
    }
}