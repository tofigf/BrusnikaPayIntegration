using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrusnikaPayIntegration.Models.Requests
{
    public record PaymentFormFullRequest
    {
        public string ClientID { get; init; } = string.Empty;
        public string ClientIP { get; init; } = string.Empty;
        public DateTime ClientDateCreated { get; init; }
        public string PaymentMethod { get; init; } = string.Empty; 
        public string IdTransactionMerchant { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public IntegrationMerchantData? IntegrationMerhcnatData { get; init; }
    }

    public record IntegrationMerchantData
    {
        public string? WebHook { get; init; }
        public string? RedirectGeneralURL { get; init; }
        public string? RedirectSuccessURL { get; init; }
        public string? RedirectFailURL { get; init; }
    }
}
