using Domain.VatCalculator.Entities;
using Domain.VatCalculator.Interfaces.Validator;
using GlobalVatCalculator.API.Constants.Messages;
using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Validators.Interface;
using System.ComponentModel.DataAnnotations;

namespace GlobalVatCalculator.API.Validators;

public class PriceRequestValidator(IPriceValidator priceValidator, IVatRateValidator vatRateValidator) : IPriceRequestValidator
{
    private readonly IPriceValidator _priceValidator = priceValidator;
    private readonly IVatRateValidator _vatRateValidator = vatRateValidator;

    public IEnumerable<ValidationResult> Validate(PriceRequest priceRequest)
    {
        List<ValidationResult> validationResults = [];
        VatRate vatRate = new(priceRequest.VATRate);

        if (!_vatRateValidator.Validate(vatRate))
        {
            validationResults.Add(new ValidationResult(
                ValidationMessages.VatRateInvalid,
                [nameof(priceRequest.VATRate)]));
        }

        return validationResults;

        // TODO : Use on IEndpointFilter

        //var vr = new ValidationResult(
        //        "At least one of the values (NetValue, GrossValue, VATValue) must be provided.",
        //        [nameof(NetValue), nameof(GrossValue), nameof(VATValue)]);
        //var validationResults = new List<ValidationResult>();
        //Validator.TryValidateObject(this, validationContext, validationResults);
    }
}