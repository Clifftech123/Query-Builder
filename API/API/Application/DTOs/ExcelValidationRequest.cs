namespace API.Application.DTOs
{
    /// <summary>
    /// Request to validate Excel structure
    /// </summary>
    public class ExcelValidationRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
