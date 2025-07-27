using Serilog;

namespace GlobalVatCalculator.API.Extensions;

public static class LoggerExtension
{
    public static ILoggerFactory AddSerilog(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Debug()
            .CreateLogger();

        var loggerFactory = new LoggerFactory();
        loggerFactory.AddSerilog(logger);

        builder.Services.AddSingleton<ILoggerFactory>(loggerFactory);

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
        builder.Host.UseSerilog(logger);

        return loggerFactory;
    }
}