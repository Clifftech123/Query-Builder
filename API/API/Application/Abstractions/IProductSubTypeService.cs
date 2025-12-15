using API.Application.DTOs;

namespace API.Application.Abstractions
{
    public interface IProductSubTypeService
    {
        Task<List<ProductSubTypeResponse>> GetAllAsync();
        Task<ProductSubTypeResponse?> GetByIdAsync(Guid id);
        Task<ProductSubTypeResponse> CreateAsync(CreateProductSubTypeRequest request);
        Task<ProductSubTypeResponse?> UpdateAsync(Guid id, UpdateProductSubTypeRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
