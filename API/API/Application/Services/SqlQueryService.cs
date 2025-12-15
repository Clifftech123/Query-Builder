using API.Application.Abstractions;
using API.Application.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Application.Services
{
    /// <summary>
    /// Service for executing raw SQL queries like SQL Server Management Studio
    /// </summary>
    public class SqlQueryService : ISqlQueryService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SqlQueryService> _logger;

        public SqlQueryService(IConfiguration configuration, ILogger<SqlQueryService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Executes raw SQL query and returns results
        /// </summary>
        public async Task<ExecuteSqlQueryResponse> ExecuteQueryAsync(string query)
        {
            var response = new ExecuteSqlQueryResponse();

            if (string.IsNullOrWhiteSpace(query))
            {
                response.Success = false;
                response.Message = "Query cannot be empty";
                return response;
            }

            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand(query, connection);
                command.CommandTimeout = 300; // 5 minutes

                using var reader = await command.ExecuteReaderAsync();

                // Get column names
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    response.Columns.Add(reader.GetName(i));
                }

                // Read all rows
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[reader.GetName(i)] = value;
                    }
                    response.Rows.Add(row);
                }

                response.Success = true;
                response.RowCount = response.Rows.Count;
                response.Message = $"Query executed successfully. {response.RowCount} rows returned.";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SQL query");
                response.Success = false;
                response.Message = "Error executing query";
                response.ErrorMessage = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// Gets database metadata - tables, columns, configurations
        /// </summary>
        public async Task<DatabaseMetadataResponse> GetDatabaseMetadataAsync()
        {
            var response = new DatabaseMetadataResponse();

            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Get all user tables
                var query = @"
                    SELECT
                        t.TABLE_NAME,
                        c.COLUMN_NAME,
                        c.DATA_TYPE,
                        c.IS_NULLABLE
                    FROM INFORMATION_SCHEMA.TABLES t
                    INNER JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME
                    WHERE t.TABLE_TYPE = 'BASE TABLE'
                    AND t.TABLE_NAME NOT IN ('__EFMigrationsHistory')
                    ORDER BY t.TABLE_NAME, c.ORDINAL_POSITION";

                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                var tableDict = new Dictionary<string, TableInfo>();

                while (await reader.ReadAsync())
                {
                    var tableName = reader.GetString(0);
                    var columnName = reader.GetString(1);
                    var dataType = reader.GetString(2);
                    var isNullable = reader.GetString(3) == "YES";

                    if (!tableDict.ContainsKey(tableName))
                    {
                        tableDict[tableName] = new TableInfo
                        {
                            TableName = tableName,
                            Columns = []
                        };
                    }

                    tableDict[tableName].Columns.Add(new ColumnInfo
                    {
                        ColumnName = columnName,
                        DataType = dataType,
                        IsNullable = isNullable
                    });
                }

                response.Tables = tableDict.Values.ToList();

                // Get configurations
                var configQuery = "SELECT Id, Name FROM Configurations WHERE DeletedAt IS NULL";
                using var configCommand = new SqlCommand(configQuery, connection);
                using var configReader = await configCommand.ExecuteReaderAsync();

                while (await configReader.ReadAsync())
                {
                    response.Configurations.Add(new ConfigurationInfo
                    {
                        Id = configReader.GetGuid(0),
                        Name = configReader.GetString(1)
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database metadata");
                return response;
            }
        }
    }
}
