namespace BrusnikaPayIntegration.Models
{
    public class PaymentFormRequest
    {
        public string IdTransactionMerchant { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
        public string? WebhookUrl { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Description { get; set; }
    }
}
