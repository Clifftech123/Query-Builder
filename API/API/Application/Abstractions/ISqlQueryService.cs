using API.Application.DTOs;

namespace API.Application.Abstractions
{
    public interface ISqlQueryService
    {
        Task<ExecuteSqlQueryResponse> ExecuteQueryAsync(string query);
        Task<DatabaseMetadataResponse> GetDatabaseMetadataAsync();
    }
}
