using API.Application.Abstractions;
using API.Application.Common;
using API.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductResponse>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<ProductResponse>>.CreateSuccess(result, "Products retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse<ProductResponse>.CreateFail("Product not found", 404));

            return Ok(ApiResponse<ProductResponse>.CreateSuccess(result, "Product retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> Create(CreateProductRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<ProductResponse>.CreateSuccess(result, "Product created successfully", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponse>>> Update(Guid id, UpdateProductRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result == null)
                return NotFound(ApiResponse<ProductResponse>.CreateFail("Product not found", 404));

            return Ok(ApiResponse<ProductResponse>.CreateSuccess(result, "Product updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.CreateFail("Product not found", 404));

            return Ok(ApiResponse<object>.CreateSuccess(null!, "Product deleted successfully"));
        }
    }
}
