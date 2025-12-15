namespace API.Domain.ValueObjects
{
    /// <summary>
    /// Metadata information about an Excel column
    /// </summary>
    public class ColumnMetadata
    {
        public string OriginalName { get; set; } = string.Empty;
        public int ColumnNumber { get; set; }
        public Type DataType { get; set; } = typeof(string);
        public string SqlDataType { get; set; } = "NVARCHAR(MAX)";
    }
}
