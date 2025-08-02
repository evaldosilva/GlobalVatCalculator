using Domain.VatCalculator.Interfaces.Service;
using GlobalVatCalculator.API.Constants.Routes;
using GlobalVatCalculator.API.Filters;
using GlobalVatCalculator.API.Mappings;
using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GlobalVatCalculator.API.Controllers;

[ApiController]
[Route(RouteDefinitions.V1.Base)]
public class VatCalculatorController(IVatCalculator vatCalculator) : ControllerBase
{
    private readonly IVatCalculator _vatCalculator = vatCalculator;

    [HttpGet(RouteDefinitions.V1.PriceCalculatorEndpoint)]
    [ProducesResponseType(typeof(PriceResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [EndpointDescription(RouteDefinitions.V1.PriceCalculatorEndpointDesc)]
    [ServiceFilter(typeof(PriceCalculatorFilter))]
    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = ["*"])]
    public async Task<IActionResult> PriceCalculator([FromQuery] PriceRequest priceRequest, CancellationToken cancellationToken)
    {
        var price = PriceRequestMapper.MapPriceRequestToPrice(priceRequest);
        var calculatedPrice = await _vatCalculator.CalculateVat(price);

        cancellationToken.ThrowIfCancellationRequested();

        return Ok(PriceResultMapper.MapPriceToPriceResult(calculatedPrice));
    }
}