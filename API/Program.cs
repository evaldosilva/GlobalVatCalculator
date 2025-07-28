using GlobalVatCalculator.API.Extensions;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using static GlobalVatCalculator.API.Constants.Routes.RouteDefinitions;

var builder = WebApplication.CreateBuilder(args);

var loggerFactory = builder.AddSerilog();
var log = loggerFactory.CreateLogger(AppName);
log.LogInformation("Starting {AppName}", AppName);

// Validate service scopes on build
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

try
{
    builder.Services.AddControllers();

    // Configure OpenAPI
    builder.Services.AddOutputCache(options =>
    {
        options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(120)));
    });

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info.Title = V1.RouteTitle;
            document.Info.Version = V1.Version;
            document.Info.Description = V1.Description;
            document.Info.Contact = new OpenApiContact
            {
                Name = ContactInfo.Name,
                Url = ContactInfo.Url
            };
            return Task.CompletedTask;
        });
    });

    // Configure Custom services
    builder.Services.AddRequestValidators();
    builder.Services.AddBusinessServices();

    // Enrich ProblemDetails
    builder.Services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = (context) =>
        {
            context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        };
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseOutputCache();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi().CacheOutput();
        app.MapScalarApiReference(options =>
        {
            options.WithTheme(ScalarTheme.BluePlanet)
            .WithTitle(V1.RouteTitle)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    log.LogCritical(ex, "An error occurred while starting the application {AppName}.", AppName);
    throw;
}
finally
{
    log.LogInformation("Stopping {AppName}", AppName);
}

public partial class Program { }