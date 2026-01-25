// @Bruno Milagres - 2024-06-07
// Program.cs
// Main entry point. Configures logging (Serilog), tracing (OpenTelemetry)
// and exposes OpenAPI + Scalar UI at "/"

using Microsoft.OpenApi;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Serilog
// --------------------
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// --------------------
// OpenTelemetry (Tracing)
// --------------------
builder.Services.AddOpenTelemetry()
    .WithTracing(tracer =>
    {
        tracer
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(
                        serviceName: "MyProject.Api",
                        serviceVersion: "1.0.0"))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    });

// --------------------
// OpenAPI
// --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "MyProject API",
            Version = "v1",
            Description = "OpenAPI specification"
        };

        return Task.CompletedTask;
    });
});

var app = builder.Build();

// --------------------
// Middleware pipeline
// --------------------
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

// --------------------
// OpenAPI JSON
// --------------------
app.MapOpenApi("/openapi/v1.json");

// --------------------
// Scalar UI (OPENAPI UI)
// Executed at "/"
// --------------------
app.MapScalarApiReference("/", options =>
{
    options
        .WithTitle("MyProject API")
        .WithTheme(ScalarTheme.Moon)
        .WithOpenApiRoutePattern("/openapi/{documentName}.json");
});

// --------------------
// Endpoint Health
// --------------------
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy"
}));

app.Run();