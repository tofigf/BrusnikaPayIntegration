using System.ComponentModel.DataAnnotations;

namespace BrusnikaPayIntegration.Models.Requests
{
    public record PaymentFormRequest
    {
        [Required]
        public string IdTransactionMerchant { get; init; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; init; }

        [Required]
        public string Currency { get; init; } = "RUB";
        public string? Description { get; init; }
    } 
}
