using API.Domain.ValueObjects;
using OfficeOpenXml;
using System.Data;

namespace API.Infrastructure.Excel
{
    /// <summary>
    /// Reads Excel data and implements IDataReader for bulk operations
    /// </summary>
    public class ExcelTransactionReader : IDataReader
    {
        private readonly ExcelWorksheet _worksheet;
        private readonly Dictionary<string, ColumnMetadata> _columns;
        private readonly List<string> _columnNames;
        private readonly int _startRow;
        private readonly int _endRow;
        private int _currentRow;
        private bool _disposed;

        public Dictionary<string, ColumnMetadata> Columns => _columns;

        public ExcelTransactionReader(ExcelWorksheet worksheet, int headerRow = 1, bool detectDataTypes = true)
        {
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
            _startRow = headerRow + 1;
            _endRow = worksheet.Dimension?.End.Row ?? 0;
            _currentRow = _startRow - 1;

            _columns = [];
            _columnNames = [];

            if (worksheet.Dimension != null)
            {
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    var headerValue = worksheet.Cells[headerRow, col].Value?.ToString() ?? $"Column{col}";
                    var sanitizedName = SanitizeColumnName(headerValue, $"{col}");

                    var uniqueName = GetUniqueColumnName(sanitizedName);

                    var metadata = new ColumnMetadata
                    {
                        OriginalName = headerValue,
                        ColumnNumber = col,
                        DataType = detectDataTypes ? DetectDataType(col) : typeof(string),
                    };
                    metadata.SqlDataType = MapToSqlType(metadata.DataType);

                    _columns[uniqueName] = metadata;
                    _columnNames.Add(uniqueName);
                }
            }
        }

        public int FieldCount => _columns.Count;
        public object this[int i] => GetValue(i);
        public object this[string name] => GetValue(GetOrdinal(name));
        public int Depth => 0;
        public bool IsClosed => _disposed;
        public int RecordsAffected => -1;

        private string MapToSqlType(Type dataType)
        {
            if (dataType == typeof(int)) return "INT";
            if (dataType == typeof(long)) return "BIGINT";
            if (dataType == typeof(decimal) || dataType == typeof(double) || dataType == typeof(float))
                return "DECIMAL(18,2)";
            if (dataType == typeof(DateTime)) return "DATETIME";
            if (dataType == typeof(bool)) return "BIT";
            if (dataType == typeof(Guid)) return "UNIQUEIDENTIFIER";

            return "NVARCHAR(MAX)";
        }

        private string GetUniqueColumnName(string baseName)
        {
            if (!_columns.ContainsKey(baseName))
                return baseName;

            int counter = 1;
            string uniqueName;
            do
            {
                uniqueName = $"{baseName}_{counter}";
                counter++;
            } while (_columns.ContainsKey(uniqueName));

            return uniqueName;
        }

        public bool Read()
        {
            if (_currentRow >= _endRow)
                return false;

            _currentRow++;
            return _currentRow <= _endRow;
        }

        public object GetValue(int i)
        {
            if (i < 0 || i >= _columnNames.Count)
                throw new IndexOutOfRangeException();

            var columnName = _columnNames[i];
            var metadata = _columns[columnName];
            var cellValue = _worksheet.Cells[_currentRow, metadata.ColumnNumber].Value;

            if (cellValue == null)
                return DBNull.Value;

            try
            {
                if (metadata.DataType == typeof(DateTime))
                {
                    if (cellValue is double oleDateValue && LooksLikeExcelOADate(oleDateValue))
                    {
                        return DateTime.FromOADate(oleDateValue);
                    }
                    return Convert.ToDateTime(cellValue);
                }

                if (metadata.DataType == typeof(int)) return Convert.ToInt32(cellValue);
                if (metadata.DataType == typeof(decimal)) return Convert.ToDecimal(cellValue);
                if (metadata.DataType == typeof(bool)) return Convert.ToBoolean(cellValue);

                return cellValue.ToString();
            }
            catch
            {
                return cellValue.ToString();
            }
        }

        private bool LooksLikeExcelOADate(double value) => value > 20000 && value < 60000;

        public string GetName(int i)
        {
            if (i < 0 || i >= _columnNames.Count)
                throw new IndexOutOfRangeException();
            return _columnNames[i];
        }

        public int GetOrdinal(string name)
        {
            var index = _columnNames.IndexOf(name);
            if (index == -1)
                throw new IndexOutOfRangeException($"Column '{name}' not found");
            return index;
        }

        public string GetDataTypeName(int i) => GetFieldType(i).Name;

        public Type GetFieldType(int i)
        {
            var columnName = _columnNames[i];
            return _columns[columnName].DataType;
        }

        public bool IsDBNull(int i)
        {
            var value = GetValue(i);
            return value == null || value == DBNull.Value;
        }

        public int GetValues(object[] values)
        {
            int count = Math.Min(values.Length, _columnNames.Count);
            for (int i = 0; i < count; i++)
                values[i] = GetValue(i);
            return count;
        }

        public DataTable GetSchemaTable()
        {
            var schemaTable = new DataTable();
            schemaTable.Columns.Add("ColumnName", typeof(string));
            schemaTable.Columns.Add("OriginalName", typeof(string));
            schemaTable.Columns.Add("ColumnOrdinal", typeof(int));
            schemaTable.Columns.Add("ColumnNumber", typeof(int));
            schemaTable.Columns.Add("DataType", typeof(Type));
            schemaTable.Columns.Add("SqlDataType", typeof(string));

            for (int i = 0; i < _columnNames.Count; i++)
            {
                var columnName = _columnNames[i];
                var metadata = _columns[columnName];

                var row = schemaTable.NewRow();
                row["ColumnName"] = columnName;
                row["OriginalName"] = metadata.OriginalName;
                row["ColumnOrdinal"] = i;
                row["ColumnNumber"] = metadata.ColumnNumber;
                row["DataType"] = metadata.DataType;
                row["SqlDataType"] = metadata.SqlDataType;
                schemaTable.Rows.Add(row);
            }

            return schemaTable;
        }

        private string SanitizeColumnName(string name, string colNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                return $"Column_{colNumber}";

            var sanitized = new string(name.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            if (string.IsNullOrEmpty(sanitized) || char.IsDigit(sanitized[0]))
                sanitized = "_" + sanitized;

            return sanitized;
        }

        private Type DetectDataType(int columnNumber)
        {
            var sampleSize = Math.Min(100, _endRow - _startRow + 1);
            var types = new Dictionary<Type, int>();

            for (int row = _startRow; row < _startRow + sampleSize && row <= _endRow; row++)
            {
                var value = _worksheet.Cells[row, columnNumber].Value;
                if (value == null) continue;

                var detectedType = TryParseValue(value);
                if (!types.ContainsKey(detectedType))
                    types[detectedType] = 0;
                types[detectedType]++;
            }

            if (types.Count == 0) return typeof(string);

            var dominantType = types.OrderByDescending(x => x.Value).First().Key;
            var totalValues = types.Values.Sum();

            if (types[dominantType] < totalValues * 0.8)
                return typeof(string);

            return dominantType;
        }

        private Type TryParseValue(object cellValue)
        {
            try
            {
                return cellValue switch
                {
                    DateTime => typeof(DateTime),
                    double d when LooksLikeExcelOADate(d) => typeof(DateTime),
                    double => typeof(decimal),
                    decimal => typeof(decimal),
                    int => typeof(int),
                    _ => typeof(string)
                };
            }
            catch
            {
                return typeof(string);
            }
        }

        public void Close() => Dispose();

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        public bool NextResult() => false;
        public bool GetBoolean(int i) => Convert.ToBoolean(GetValue(i));
        public byte GetByte(int i) => Convert.ToByte(GetValue(i));
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => 0;
        public char GetChar(int i) => Convert.ToChar(GetValue(i));
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => 0;
        public IDataReader GetData(int i) => throw new NotImplementedException();
        public DateTime GetDateTime(int i) => Convert.ToDateTime(GetValue(i));
        public decimal GetDecimal(int i) => Convert.ToDecimal(GetValue(i));
        public double GetDouble(int i) => Convert.ToDouble(GetValue(i));
        public float GetFloat(int i) => Convert.ToSingle(GetValue(i));
        public Guid GetGuid(int i) => Guid.Parse(GetValue(i).ToString()!);
        public short GetInt16(int i) => Convert.ToInt16(GetValue(i));
        public int GetInt32(int i) => Convert.ToInt32(GetValue(i));
        public long GetInt64(int i) => Convert.ToInt64(GetValue(i));
        public string GetString(int i) => GetValue(i)?.ToString() ?? string.Empty;
    }
}
