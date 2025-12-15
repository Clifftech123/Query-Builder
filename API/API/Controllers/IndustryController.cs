using API.Application.Abstractions;
using API.Application.Common;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndustryController : ControllerBase
    {
        private readonly IIndustryService _service;

        public IndustryController(IIndustryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<IndustryResponse>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<IndustryResponse>>.CreateSuccess(result, "Industries retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<IndustryResponse>>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<IndustryResponse>.CreateFail("Industry not found", 404));

            return Ok(ApiResponse<IndustryResponse>.CreateSuccess(result, "Industry retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<IndustryResponse>>> Create(CreateIndustryRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<IndustryResponse>.CreateSuccess(result, "Industry created successfully", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<IndustryResponse>>> Update(Guid id, UpdateIndustryRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null)
                return NotFound(ApiResponse<IndustryResponse>.CreateFail("Industry not found", 404));

            return Ok(ApiResponse<IndustryResponse>.CreateSuccess(result, "Industry updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.CreateFail("Industry not found", 404));

            return Ok(ApiResponse<object>.CreateSuccess(null!, "Industry deleted successfully"));
        }
    }
}
