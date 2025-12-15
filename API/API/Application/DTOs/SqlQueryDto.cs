namespace API.Application.DTOs
{
    public class ExecuteSqlQueryRequest
    {
        public string Query { get; set; } = string.Empty;
    }

    public class ExecuteSqlQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RowCount { get; set; }
        public List<string> Columns { get; set; } = [];
        public List<Dictionary<string, object?>> Rows { get; set; } = [];
        public string? ErrorMessage { get; set; }
    }

    public class DatabaseMetadataResponse
    {
        public List<TableInfo> Tables { get; set; } = [];
        public List<ConfigurationInfo> Configurations { get; set; } = [];
    }

    public class TableInfo
    {
        public string TableName { get; set; } = string.Empty;
        public List<ColumnInfo> Columns { get; set; } = [];
    }

    public class ColumnInfo
    {
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
    }

    public class ConfigurationInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SaveQueryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class SavedQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
