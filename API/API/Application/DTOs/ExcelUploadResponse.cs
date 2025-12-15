namespace API.Application.DTOs
{
    /// <summary>
    /// Response after Excel upload
    /// </summary>
    public class ExcelUploadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? TableName { get; set; }
        public int RowCount { get; set; }
        public List<ExcelColumnInfo> Columns { get; set; } = [];
        public List<string>? ValidationErrors { get; set; }
    }

    public class ExcelColumnInfo
    {
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public int ColumnNumber { get; set; }
    }
}
