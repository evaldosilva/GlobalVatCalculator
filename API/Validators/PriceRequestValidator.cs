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

    public ValueTask<IEnumerable<ValidationResult>> Validate(PriceRequest priceRequest)
    {
        List<ValidationResult> validationResults = [];
        bool IsSomeAmountValid = false;

        if (priceRequest?.NetValue == null || priceRequest?.NetValue == 0)
            validationResults.Add(new ValidationResult(
                string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.NetValue, nameof(priceRequest.NetValue)),
                [nameof(priceRequest.NetValue)]));
        else
            IsSomeAmountValid = true;

        if (IsSomeAmountValid is false)
            if (priceRequest?.VATValue == null || priceRequest?.VATValue == 0)
                validationResults.Add(new ValidationResult(
                    string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.VATValue, nameof(priceRequest.VATValue)),
                    [nameof(priceRequest.VATValue)]));
            else
                IsSomeAmountValid = true;

        if (IsSomeAmountValid is false)
            if (priceRequest?.GrossValue == null || priceRequest?.GrossValue == 0)
                validationResults.Add(new ValidationResult(
                    string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.GrossValue, nameof(priceRequest.GrossValue)),
                    [nameof(priceRequest.GrossValue)]));
            else
                IsSomeAmountValid = true;

        if (IsSomeAmountValid)
            validationResults.Clear();

        if (priceRequest?.VATRate == 0)
            validationResults.Add(new ValidationResult(
                string.Format(ValidationMessages.InvalidNonNumeric, priceRequest?.VATRate, nameof(priceRequest.VATRate)),
                [nameof(priceRequest.VATRate)]));

        if (validationResults.Count is not 0)
            return ValueTask.FromResult<IEnumerable<ValidationResult>>(validationResults);

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

        return ValueTask.FromResult<IEnumerable<ValidationResult>>(validationResults); ;
    }
}