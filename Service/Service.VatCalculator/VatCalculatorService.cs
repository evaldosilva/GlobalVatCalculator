using Domain.VatCalculator.Interfaces.Calculator;
using Domain.VatCalculator.Interfaces.Service;
using Domain.VatCalculator.Interfaces.Validator;
using Domain.VatCalculator.Models;
using Domain.VatCalculator.Types;
using Service.VatCalculator.Calculators;
using Service.VatCalculator.Validator;

namespace Service.VatCalculator;

public class VatCalculatorService(IEnumerable<IPriceValidationHandler> priceValidationHandlers) : IVatCalculator
{
    private readonly IEnumerable<IPriceValidationHandler> _priceValidationHandlers = priceValidationHandlers;

    public async ValueTask<Price> CalculateVat(Price price)
    {
        IPriceValidationHandler validators = _priceValidationHandlers
            .First(h => h.Type == PriceValidationHandlerType.PriceMissingValidator);

        validators
            .SetNext(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceMultipleInputValidator))
            .SetNext(_priceValidationHandlers.First(h => h.Type == PriceValidationHandlerType.PriceVATTaxRateValidator));

        PriceValidator priceValidation = new(validators);
        priceValidation.Validate(price);

        ICalculator calculator;
        if (priceValidation.IsValid())
            if (price.NetValue.HasValue && price.NetValue > 0)
                calculator = CalculatorFactory.GetCalculator(CalculationType.NetValueCalculation);
            else if (price.GrossValue.HasValue && price.GrossValue > 0)
                calculator = CalculatorFactory.GetCalculator(CalculationType.GrossValueCalculation);
            else if (price.VATValue.HasValue && price.VATValue > 0)
                calculator = CalculatorFactory.GetCalculator(CalculationType.VATValueCalculation);
            else
                calculator = CalculatorFactory.GetCalculator();
        else
            calculator = CalculatorFactory.GetCalculator();

        return await calculator.Calculate(price);
    }
}