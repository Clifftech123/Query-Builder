namespace API.Application.DTOs
{
    /// <summary>
    /// Request for uploading Excel file
    /// </summary>
    public class ExcelUploadRequest
    {
        public IFormFile File { get; set; } = null!;
        public Guid ConfigurationId { get; set; }
    }
}
