using BrusnikaPayIntegration.Extensions;
using BrusnikaPayIntegration.Models.Requests;
using BrusnikaPayIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddBrusnikaPayServices(builder.Configuration);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var host = builder.Build();

await using var scope = host.Services.CreateAsyncScope();
var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    if (!paymentService.IsAuthenticated)
    {
        logger.LogError("JWT token is not configured. Please set BrusnikaPay:JwtToken in environment variables.");
        return;
    }

    var paymentFormRequest = new PaymentFormRequest
    {
        IdTransactionMerchant = $"FORM_{DateTime.Now:yyyyMMdd_HHmmss}",
        Amount = 500.00m,
        Currency = "RUB",
        Description = "Test payment form"
    };

    var formResponse = await paymentService.CreatePaymentFormAsync(paymentFormRequest);

    if (formResponse?.Result.Status == "success" && formResponse.Data != null)
    {
        logger.LogInformation("Payment form created successfully!");
        paymentService.DisplayPaymentForm(formResponse.Data);
    }
    else
    {
        logger.LogWarning($"Payment form creation failed: {formResponse?.Result.Message}");
    }
    var fullRequest = new PaymentFormFullRequest
    {
        ClientID = Guid.NewGuid().ToString(),
        ClientIP = "192.168.1.1",
        ClientDateCreated = DateTime.UtcNow.AddDays(-30),
        PaymentMethod = "sbp",
        IdTransactionMerchant = $"FULL_{DateTime.Now:yyyyMMdd_HHmmss}",
        Amount = 750.00m
    };

    var fullResponse = await paymentService.CreatePaymentFormFullAsync(fullRequest);
    if (fullResponse?.Result.Status == "success" && fullResponse.Data != null)
    {
        logger.LogInformation("Payment form full created successfully!");
        paymentService.DisplayPaymentForm(fullResponse.Data);
    }
    logger.LogInformation("Service is running. Press Ctrl+C to stop.");

    var cancellationToken = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) => {
        e.Cancel = true;
        cancellationToken.Cancel();
    };

    try
    {
        await Task.Delay(Timeout.Infinite, cancellationToken.Token);
    }
    catch (OperationCanceledException)
    {
        logger.LogInformation("Service is shutting down...");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while running the service");
}
