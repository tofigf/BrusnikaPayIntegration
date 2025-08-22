namespace BrusnikaPayIntegration.Models
{
    public class ApiResult
    {
        public string Status { get; set; } = string.Empty;
        public string XRequestId { get; set; } = string.Empty;
        public string CodeError { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
