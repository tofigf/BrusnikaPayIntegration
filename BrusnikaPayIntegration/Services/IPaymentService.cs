using BrusnikaPayIntegration.Models;
using BrusnikaPayIntegration.Models.Requests;

namespace BrusnikaPayIntegration.Services
{
    public interface IPaymentService
    {
        bool IsAuthenticated { get; }
        Task<ApiResponse<OperationDataForPaymentForm>?> CreatePaymentFormAsync(PaymentFormRequest request);
        void DisplayPaymentForm(OperationDataForPaymentForm operationData);
        Task<ApiResponse<OperationDataForPaymentForm>?> CreatePaymentFormFullAsync(PaymentFormFullRequest request);
    }
}
