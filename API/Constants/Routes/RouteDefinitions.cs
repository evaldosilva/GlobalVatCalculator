namespace GlobalVatCalculator.API.Constants.Routes;

public static class RouteDefinitions
{
    public static class ContactInfo
    {
        public const string Name = "Evaldo Silva";
        public static readonly Uri Url = new("https://github.com/evaldosilva");
    }

    public static class V1
    {
        public const string RouteTitle = "Global VAT Calculator API V1";
        public const string Description = "API for calculating VAT rates globally.";
        public const string Version = "v1";
        public const string Base = "api/v1/[controller]";
        public const string PriceCalculatorEndpoint = "PriceCalculator";
    }
}