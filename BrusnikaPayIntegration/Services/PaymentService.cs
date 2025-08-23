using BrusnikaPayIntegration.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using BrusnikaPayIntegration.Models.Requests;
using BrusnikaPayIntegration.Exceptions;
using BrusnikaPayIntegration.Configuration;
using Microsoft.Extensions.Options;

namespace BrusnikaPayIntegration.Services
{
    public class PaymentService: IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private string? _authToken;
        private readonly BrusnikaPayOptions _options;

        public PaymentService(HttpClient httpClient, IConfiguration configuration, IOptions<BrusnikaPayOptions> options, ILogger<PaymentService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _configuration = configuration;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);

            var jwtToken = _configuration["BrusnikaPay:JwtToken"];

            if (!string.IsNullOrEmpty(jwtToken))
            {
                _authToken = jwtToken;
                _httpClient.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", _authToken);
            }
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);


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


                var response = await _httpClient.PostAsync(_options.PaymentFormPrepareEndpoint, content);
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

        public async Task<ApiResponse<OperationDataForPaymentForm>?> CreatePaymentFormFullAsync(PaymentFormFullRequest request)
        {
            try
            {
                if (!IsAuthenticated)
                {
                    throw new BrusnikaPayException("Service is not authenticated", "AUTH_ERROR");
                }

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending Payment Form Full request: {Json}", json);

                var response = await _httpClient.PostAsync(_options.PaymentFormFullEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Payment Form Full Response Status: {Status}", response.StatusCode);
                _logger.LogInformation("Payment Form Full Response: {Content}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new BrusnikaPayException($"API request failed with status {response.StatusCode}", "API_ERROR");
                }

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<OperationDataForPaymentForm>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                return apiResponse;
            }
            catch (Exception ex) when (!(ex is BrusnikaPayException))
            {
                _logger.LogError(ex, "Error creating payment form full");
                throw new BrusnikaPayException("Failed to create payment form", "UNKNOWN_ERROR", ex);
            }
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
