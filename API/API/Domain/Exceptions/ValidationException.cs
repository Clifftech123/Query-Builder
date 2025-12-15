namespace API.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when domain validation fails
    /// </summary>
    public class ValidationException : DomainException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(string message, string code = "VALIDATION_ERROR")
            : base(message, code)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string propertyName, string errorMessage, string code = "VALIDATION_ERROR")
            : base(errorMessage, code)
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }

        public ValidationException(Dictionary<string, string[]> errors, string code = "VALIDATION_ERROR")
            : base("One or more validation errors occurred", code)
        {
            Errors = errors;
        }
    }
}
