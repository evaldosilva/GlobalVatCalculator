using Domain.VatCalculator.Interfaces.Service;
using Domain.VatCalculator.Validation;
using GlobalVatCalculator.API.Mappings;
using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Validators.Interface;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators;

public class PriceRequestValidator(IVatCalculator vatCalculator) : IPriceRequestValidator
{
    private readonly IVatCalculator _vatCalculator = vatCalculator;
    public async Task<IEnumerable<ValidationResult>> Validate(PriceRequest priceRequest) =>
        await Task.Run(async () =>
        {
            List<ValidationResult> validationResults = [];

            var price = PriceRequestMapper.MapPriceRequestToPrice(priceRequest);

            Result validationResult = await _vatCalculator.ValidatePrice(price);

            if (validationResult.IsFailure)
                validationResults.Add(CreateValidationResult(validationResult.Error.Description, validationResult.MemberNames));

            return validationResults;
        });

    private static ValidationResult CreateValidationResult(string? errorMessage, IEnumerable<string>? memberNames)
        => new(errorMessage, memberNames);
}