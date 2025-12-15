using API.Application.Abstractions;
using API.Application.Common;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductSubTypeController : ControllerBase
    {
        private readonly IProductSubTypeService _service;

        public ProductSubTypeController(IProductSubTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductSubTypeResponse>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<ProductSubTypeResponse>>.CreateSuccess(result, "Product subtypes retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductSubTypeResponse>>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<ProductSubTypeResponse>.CreateFail("Product subtype not found", 404));

            return Ok(ApiResponse<ProductSubTypeResponse>.CreateSuccess(result, "Product subtype retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductSubTypeResponse>>> Create(CreateProductSubTypeRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<ProductSubTypeResponse>.CreateSuccess(result, "Product subtype created successfully", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductSubTypeResponse>>> Update(Guid id, UpdateProductSubTypeRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null)
                return NotFound(ApiResponse<ProductSubTypeResponse>.CreateFail("Product subtype not found", 404));

            return Ok(ApiResponse<ProductSubTypeResponse>.CreateSuccess(result, "Product subtype updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.CreateFail("Product subtype not found", 404));

            return Ok(ApiResponse<object>.CreateSuccess(null!, "Product subtype deleted successfully"));
        }
    }
}
