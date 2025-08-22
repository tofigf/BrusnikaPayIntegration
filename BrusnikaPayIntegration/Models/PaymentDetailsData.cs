namespace BrusnikaPayIntegration.Models
{
    public class PaymentDetailsData
    {
        public string NameMediator { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string NumberAdditional { get; set; } = string.Empty;
        public string QRcode { get; set; } = string.Empty;
    }
}
