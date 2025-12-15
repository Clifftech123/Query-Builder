namespace API.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a domain operation is invalid
    /// </summary>
    public class InvalidOperationDomainException : DomainException
    {
        public InvalidOperationDomainException(string message, string code = "INVALID_OPERATION")
            : base(message, code)
        {
        }

        public InvalidOperationDomainException(string message, string code, Exception innerException)
            : base(message, code, innerException)
        {
        }
    }
}
