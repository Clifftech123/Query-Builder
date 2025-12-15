using API.Application.DTOs;

namespace API.Application.Abstractions
{
    public interface IExcelService
    {
        Task<ExcelUploadResponse> UploadExcelAsync(IFormFile file, Guid configurationId);
    }
}
