using API.Application.DTOs;

namespace API.Application.Abstractions
{
    public interface IIndustryService
    {
        Task<List<IndustryResponse>> GetAllAsync();
        Task<IndustryResponse?> GetByIdAsync(Guid id);
        Task<IndustryResponse> CreateAsync(CreateIndustryRequest request);
        Task<IndustryResponse?> UpdateAsync(Guid id, UpdateIndustryRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
