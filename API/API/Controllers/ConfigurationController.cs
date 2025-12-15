using API.Application.Abstractions;
using API.Application.Common;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _service;

        public ConfigurationController(IConfigurationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ConfigurationResponse>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<ConfigurationResponse>>.CreateSuccess(result, "Configurations retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ConfigurationResponse>>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<ConfigurationResponse>.CreateFail("Configuration not found", 404));

            return Ok(ApiResponse<ConfigurationResponse>.CreateSuccess(result, "Configuration retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ConfigurationResponse>>> Create(CreateConfigurationRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<ConfigurationResponse>.CreateSuccess(result, "Configuration created successfully", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ConfigurationResponse>>> Update(Guid id, UpdateConfigurationRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null)
                return NotFound(ApiResponse<ConfigurationResponse>.CreateFail("Configuration not found", 404));

            return Ok(ApiResponse<ConfigurationResponse>.CreateSuccess(result, "Configuration updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.CreateFail("Configuration not found", 404));

            return Ok(ApiResponse<object>.CreateSuccess(null!, "Configuration deleted successfully"));
        }
    }
}
