namespace API.Application.DTOs
{
    /// <summary>
    /// Response for Excel validation
    /// </summary>
    public class ExcelValidationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
        public List<ExcelColumnInfo> Columns { get; set; } = [];
        public bool HasRequiredColumns { get; set; }
        public string? ProductColumn { get; set; }
        public string? ProductTypeColumn { get; set; }
        public string? ProductSubTypeColumn { get; set; }
    }
}
