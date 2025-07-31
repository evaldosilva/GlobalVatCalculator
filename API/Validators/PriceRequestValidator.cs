using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Types;
using GlobalVatCalculator.API.Constants.Messages;
using GlobalVatCalculator.API.Mappings;
using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Validators.Interface;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators;

public class PriceRequestValidator(IPriceValidator priceValidator, IEnumerable<IPriceValidationHandler> priceValidationHandlers) : IPriceRequestValidator
{
    private readonly IPriceValidator _priceValidator = priceValidator;
    private readonly IEnumerable<IPriceValidationHandler> _priceValidationHandlers = priceValidationHandlers;

    public ValueTask<IEnumerable<ValidationResult>> Validate(PriceRequest priceRequest)
    {
        List<ValidationResult> validationResults = [];
        bool IsSomeAmountValid = false;

        if (priceRequest?.NetValue is null || priceRequest?.NetValue == 0)
            validationResults.Add(CreateValidationResult(
                string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.NetValue, nameof(priceRequest.NetValue)),
                [nameof(priceRequest.NetValue)]));
        else
            IsSomeAmountValid = true;

        if (IsSomeAmountValid is false)
            if (priceRequest?.VATValue is null || priceRequest?.VATValue == 0)
                validationResults.Add(CreateValidationResult(
                    string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.VATValue, nameof(priceRequest.VATValue)),
                    [nameof(priceRequest.VATValue)]));
            else
                IsSomeAmountValid = true;

        if (IsSomeAmountValid is false)
            if (priceRequest?.GrossValue is null || priceRequest?.GrossValue == 0)
                validationResults.Add(CreateValidationResult(
                    string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.GrossValue, nameof(priceRequest.GrossValue)),
                    [nameof(priceRequest.GrossValue)]));
            else
                IsSomeAmountValid = true;

        if (IsSomeAmountValid)
            validationResults.Clear();

        if (priceRequest?.VATRate == 0)
            validationResults.Add(CreateValidationResult(
                string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.VATRate, nameof(priceRequest.VATRate)),
                [nameof(priceRequest.VATRate)]));

        if (validationResults.Count is not 0)
            return ValueTask.FromResult<IEnumerable<ValidationResult>>(validationResults);

        var price = PriceRequestMapper.MapPriceRequestToPrice(priceRequest);

        _priceValidator.SetHandler(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceMissingValidator));
        if (_priceValidator.Validate(price) is not true)
            validationResults.Add(CreateValidationResult(
                ValidationMessages.PricesMissingOrInvalid,
                [nameof(priceRequest.VATValue), nameof(priceRequest.NetValue), nameof(priceRequest.GrossValue)]));

        _priceValidator.SetHandler(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceMultipleInputValidator));
        if (_priceValidator.Validate(price) is not true)
            validationResults.Add(CreateValidationResult(
                ValidationMessages.PricesMultipleInput,
                [nameof(priceRequest.VATValue), nameof(priceRequest.NetValue), nameof(priceRequest.GrossValue)]));

        _priceValidator.SetHandler(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceVATTaxRateValidator));
        if (_priceValidator.Validate(price) is not true)
            validationResults.Add(CreateValidationResult(
                ValidationMessages.VatRateInvalid,
                [nameof(priceRequest.VATRate)]));

        return ValueTask.FromResult<IEnumerable<ValidationResult>>(validationResults); ;
    }

    private static ValidationResult CreateValidationResult(string? errorMessage, IEnumerable<string>? memberNames)
        => new(errorMessage, memberNames);
}