namespace BrusnikaPayIntegration.Configuration
{
    public class BrusnikaPayOptions
    {
        public const string SectionName = "BrusnikaPay";
        public string BaseUrl { get; set; } = "https://api.brusnikapay.top/";
        public string JwtToken { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;

        public string PaymentFormPrepareEndpoint { get; set; } = "paymentform/prepare";
        public string PaymentFormFullEndpoint { get; set; } = "paymentform/full";
    }
}
