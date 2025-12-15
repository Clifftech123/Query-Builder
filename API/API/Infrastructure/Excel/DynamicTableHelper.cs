using API.Domain.ValueObjects;
using Microsoft.Data.SqlClient;

namespace API.Infrastructure.Excel
{
    /// <summary>
    /// Helper for creating dynamic SQL tables and bulk inserting data
    /// </summary>
    public class DynamicTableHelper
    {
        public static string GenerateCreateTableScript(Dictionary<string, ColumnMetadata> columns, string tableName)
        {
            var columnDefinitions = new List<string>();

            foreach (var kvp in columns)
            {
                var columnName = kvp.Key;
                var metadata = kvp.Value;
                columnDefinitions.Add($"[{columnName}] {metadata.SqlDataType} NULL");
            }

            return $@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')
            BEGIN
                CREATE TABLE [{tableName}] (
                    [Id] INT IDENTITY(1,1) PRIMARY KEY,
                    {string.Join(",\n        ", columnDefinitions)}
                )
            END";
        }

        public static void BulkInsert(string connectionString, string tableName, ExcelTransactionReader reader)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var createTableScript = GenerateCreateTableScript(reader.Columns, tableName);
                using (var cmd = new SqlCommand(createTableScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.BulkCopyTimeout = 300;

                    foreach (var columnName in reader.Columns.Keys)
                    {
                        bulkCopy.ColumnMappings.Add(columnName, columnName);
                    }

                    bulkCopy.WriteToServer(reader);
                }
            }
        }
    }
}
