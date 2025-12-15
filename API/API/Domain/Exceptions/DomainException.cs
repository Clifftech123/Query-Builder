namespace API.Domain.Exceptions
{
    /// <summary>
    /// Base exception for all domain-related exceptions
    /// </summary>
    public abstract class DomainException : Exception
    {
        public string Code { get; }

        protected DomainException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        protected DomainException(string message, string code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}
