using BrusnikaPayIntegration.Configuration;
using BrusnikaPayIntegration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace BrusnikaPayIntegration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrusnikaPayServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient<IPaymentService, PaymentService>()
                .AddPolicyHandler(GetRetryPolicy());

            services.Configure<BrusnikaPayOptions>(
                configuration.GetSection(BrusnikaPayOptions.SectionName));

            return services;
        }
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
