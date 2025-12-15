using API.Application.Abstractions;
using API.Application.Common;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;

        }

        /// <summary>
        /// Uploads Excel file to database - automatically adds configuration data (Industry, Product, ProductSubType) as first 3 columns
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<ExcelUploadResponse>>> UploadExcel([FromForm] IFormFile file, [FromForm] Guid configurationId)
        {
            var response = await _excelService.UploadExcelAsync(file, configurationId);

            if (!response.Success)
                return BadRequest(ApiResponse<ExcelUploadResponse>.CreateFail(response.Message, 400));

            return Ok(ApiResponse<ExcelUploadResponse>.CreateSuccess(response, "Excel uploaded successfully"));
        }
    }
}
