using API.Application.Abstractions;
using API.Application.Common;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SqlQueryController : ControllerBase
    {
        private readonly ISqlQueryService _service;

        public SqlQueryController(ISqlQueryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Executes raw SQL query (like SQL Server Management Studio)
        /// </summary>
        [HttpPost("execute")]
        public async Task<ActionResult<ApiResponse<ExecuteSqlQueryResponse>>> ExecuteQuery([FromBody] ExecuteSqlQueryRequest request)
        {
            var response = await _service.ExecuteQueryAsync(request.Query);

            if (!response.Success)
                return BadRequest(ApiResponse<ExecuteSqlQueryResponse>.CreateFail(response.Message, 400));

            return Ok(ApiResponse<ExecuteSqlQueryResponse>.CreateSuccess(response, "Query executed successfully"));
        }

        /// <summary>
        /// Gets database metadata - tables, columns, configurations
        /// </summary>
        [HttpGet("metadata")]
        public async Task<ActionResult<ApiResponse<DatabaseMetadataResponse>>> GetMetadata()
        {
            var response = await _service.GetDatabaseMetadataAsync();
            return Ok(ApiResponse<DatabaseMetadataResponse>.CreateSuccess(response, "Metadata retrieved successfully"));
        }
    }
}
