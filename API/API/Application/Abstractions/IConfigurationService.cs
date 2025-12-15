using API.Application.DTOs;

namespace API.Application.Abstractions
{
    public interface IConfigurationService
    {
        Task<List<ConfigurationResponse>> GetAllAsync();
        Task<ConfigurationResponse?> GetByIdAsync(Guid id);
        Task<ConfigurationResponse> CreateAsync(CreateConfigurationRequest request);
        Task<ConfigurationResponse?> UpdateAsync(Guid id, UpdateConfigurationRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
