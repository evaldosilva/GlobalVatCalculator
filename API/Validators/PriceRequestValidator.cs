using Domain.VatCalculator.Interfaces.Validator;
using GlobalVatCalculator.API.Constants.Messages;
using GlobalVatCalculator.API.Mappings;
using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Validators.Interface;
using Service.VatCalculator.Validator;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators;

public class PriceRequestValidator(IPriceValidator priceValidator) : IPriceRequestValidator
{
    private readonly IPriceValidator _priceValidator = priceValidator;

    public IEnumerable<ValidationResult> Validate(PriceRequest priceRequest)
    {
        List<ValidationResult> validationResults = [];
        var price = PriceRequestMapper.MapPriceRequestToPrice(priceRequest);

        _priceValidator.Validate(price);
        if (_priceValidator.IsValid() is not true)
            validationResults.Add(new ValidationResult(
                ValidationMessages.PricesMissingOrInvalid,
                [nameof(priceRequest.VATValue), nameof(priceRequest.NetValue), nameof(priceRequest.GrossValue)]));

        _priceValidator.SetHandler(new PriceMultipleInputValidator());
        _priceValidator.Validate(price);
        if (_priceValidator.IsValid() is not true)
            validationResults.Add(new ValidationResult(
                ValidationMessages.PricesMultipleInput,
                [nameof(priceRequest.VATValue), nameof(priceRequest.NetValue), nameof(priceRequest.GrossValue)]));

        _priceValidator.SetHandler(new PriceVATTaxRateValidator());
        _priceValidator.Validate(price);
        if (_priceValidator.IsValid() is not true)
            validationResults.Add(new ValidationResult(
                ValidationMessages.VatRateInvalid,
                [nameof(priceRequest.VATRate)]));

        return validationResults;
    }
}