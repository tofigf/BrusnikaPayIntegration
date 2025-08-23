namespace BrusnikaPayIntegration.Exceptions
{
    public class BrusnikaPayException : Exception
    {
        public string? ErrorCode { get; }

        public BrusnikaPayException(string message, string? errorCode = null)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public BrusnikaPayException(string message, string? errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
