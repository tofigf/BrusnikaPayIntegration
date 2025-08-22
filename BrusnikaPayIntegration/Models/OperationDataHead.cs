namespace BrusnikaPayIntegration.Models
{
    public class OperationDataHead
    {
        public string Id { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public string TypeOperation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string IdTransactionMerchant { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal AmountComission { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal AmountInCurrencyBalance { get; set; }
        public decimal AmountComissionInCurrencyBalance { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
