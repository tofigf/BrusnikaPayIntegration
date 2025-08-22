using BrusnikaPayIntegration.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BrusnikaPayIntegration.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private string? _authToken;

        public PaymentService(HttpClient httpClient, IConfiguration configuration, ILogger<PaymentService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://api.brusnikapay.top/");

            var jwtToken = _configuration["BrusnikaPay:JwtToken"];

            if (!string.IsNullOrEmpty(jwtToken))
            {
                _authToken = jwtToken;
                _httpClient.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", _authToken);
            }
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);

        /// Create PayIn service "status":"warning"
        public async Task<ApiResponse<OperationData>?> CreatePayInOperationAsync(PayInRequest request)
        {
            try
            {
                if (!IsAuthenticated)
                {
                    _logger.LogError("Service is not authenticated. Please check JWT token configuration.");
                    return null;
                }

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("host2host/payin", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<OperationData>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return apiResponse;
                }
                else
                {
                    _logger.LogError($"Failed to create PayIn operation. Status: {response.StatusCode}, Content: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PayIn operation");
            }

            return null;
        }

        public async Task<ApiResponse<OperationDataForPaymentForm>?> CreatePaymentFormAsync(PaymentFormRequest request)
        {
            try
            {
                if (!IsAuthenticated)
                {
                    _logger.LogError("Service is not authenticated. Please check JWT token configuration.");
                    return null;
                }

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _httpClient.PostAsync("paymentform/prepare", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<OperationDataForPaymentForm>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return apiResponse;
                }
                else
                {
                    _logger.LogError($"Failed to create payment form. Status: {response.StatusCode}, Content: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment form");
            }

            return null;
        }

        public void DisplayPaymentForm(OperationDataForPaymentForm operationData)
        {
            Console.WriteLine("== Payment Form == ");
            Console.WriteLine($"Operation ID: {operationData.Id}");
            Console.WriteLine($"Status: {operationData.Status}");
            Console.WriteLine($"Amount: {operationData.Amount} {operationData.Currency}");
            Console.WriteLine($"Payment Form URL: {operationData.LinkPaymentForm}");
            Console.WriteLine($"Transaction ID: {operationData.IdTransactionMerchant}");
            Console.WriteLine("====================\n");
        }
    }
}
