using API.Application.Abstractions;
using API.Application.DTOs;
using API.Infrastructure.Context;
using API.Infrastructure.Excel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace API.Application.Services
{
    /// <summary>
    /// Service for handling Excel operations
    /// </summary>
    public class ExcelService : IExcelService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExcelService> _logger;
        private readonly ApplicationDbContext _context;

        public ExcelService(IConfiguration configuration, ILogger<ExcelService> logger, ApplicationDbContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Validates Excel file structure - checks if first 3 columns are Product, ProductType, ProductSubType
        /// </summary>
        public async Task<ExcelValidationResponse> ValidateExcelAsync(IFormFile file)
        {
            var response = new ExcelValidationResponse();

            if (file == null || file.Length == 0)
            {
                response.IsValid = false;
                response.Message = "No file uploaded";
                response.Errors.Add("File is required");
                return response;
            }

            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                response.IsValid = false;
                response.Message = "Invalid file type";
                response.Errors.Add("Only .xlsx files are supported");
                return response;
            }

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];

                if (worksheet == null || worksheet.Dimension == null)
                {
                    response.IsValid = false;
                    response.Message = "Excel sheet is empty";
                    response.Errors.Add("No data found in Excel");
                    return response;
                }

                // Check first 3 columns
                var col1 = worksheet.Cells[1, 1].Value?.ToString()?.Trim() ?? "";
                var col2 = worksheet.Cells[1, 2].Value?.ToString()?.Trim() ?? "";
                var col3 = worksheet.Cells[1, 3].Value?.ToString()?.Trim() ?? "";

                bool hasProduct = col1.Equals("Product", StringComparison.OrdinalIgnoreCase);
                bool hasProductType = col2.Equals("ProductType", StringComparison.OrdinalIgnoreCase) ||
                                       col2.Equals("Product Type", StringComparison.OrdinalIgnoreCase);
                bool hasProductSubType = col3.Equals("ProductSubType", StringComparison.OrdinalIgnoreCase) ||
                                          col3.Equals("Product SubType", StringComparison.OrdinalIgnoreCase) ||
                                          col3.Equals("Product Sub Type", StringComparison.OrdinalIgnoreCase);

                response.HasRequiredColumns = hasProduct && hasProductType && hasProductSubType;
                response.ProductColumn = col1;
                response.ProductTypeColumn = col2;
                response.ProductSubTypeColumn = col3;

                if (!hasProduct)
                    response.Errors.Add("Column 1 must be 'Product'");
                if (!hasProductType)
                    response.Errors.Add("Column 2 must be 'ProductType' or 'Product Type'");
                if (!hasProductSubType)
                    response.Errors.Add("Column 3 must be 'ProductSubType' or 'Product SubType'");

                // Get all columns
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    var headerValue = worksheet.Cells[1, col].Value?.ToString() ?? $"Column{col}";
                    response.Columns.Add(new ExcelColumnInfo
                    {
                        ColumnName = headerValue,
                        ColumnNumber = col,
                        DataType = "string" // Simplified
                    });
                }

                response.IsValid = response.HasRequiredColumns && response.Errors.Count == 0;
                response.Message = response.IsValid
                    ? "Excel validation successful"
                    : "Excel validation failed";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Excel file");
                response.IsValid = false;
                response.Message = "Error reading Excel file";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        /// <summary>
        /// Uploads Excel file to database - automatically validates and adds configuration data as first 3 columns
        /// </summary>
        public async Task<ExcelUploadResponse> UploadExcelAsync(IFormFile file, Guid configurationId)
        {
            var response = new ExcelUploadResponse();

            if (file == null || file.Length == 0)
            {
                response.Success = false;
                response.Message = "No file uploaded";
                response.ValidationErrors = ["File is required"];
                return response;
            }

            string? savedFilePath = null;

            try
            {
                // Get configuration with related data
                var config = await _context.Configurations
                    .Include(c => c.Industry)
                    .Include(c => c.Product)
                    .Include(c => c.ProductSubType)
                    .FirstOrDefaultAsync(c => c.Id == configurationId);

                if (config == null)
                {
                    response.Success = false;
                    response.Message = "Configuration not found";
                    response.ValidationErrors = ["Invalid configuration ID"];
                    return response;
                }

                // Save file to disk for audit/backup purposes
                savedFilePath = await SaveFileToDiscAsync(file);

                // Read the saved file
                var fileInfo = new FileInfo(savedFilePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var package = new ExcelPackage(fileInfo);
                var worksheet = package.Workbook.Worksheets[0];

                if (worksheet == null || worksheet.Dimension == null)
                {
                    response.Success = false;
                    response.Message = "Excel sheet is empty";
                    response.ValidationErrors = ["No data found in Excel"];
                    return response;
                }

                // Insert 3 new columns at the beginning for Industry, Product, ProductSubType
                worksheet.InsertColumn(1, 3);

                // Set headers for the new columns
                worksheet.Cells[1, 1].Value = "Industry";
                worksheet.Cells[1, 2].Value = "Product";
                worksheet.Cells[1, 3].Value = "ProductSubType";

                // Fill data from configuration for all rows
                var rowCount = worksheet.Dimension.End.Row;
                for (int row = 2; row <= rowCount; row++)
                {
                    worksheet.Cells[row, 1].Value = config.Industry.Name;
                    worksheet.Cells[row, 2].Value = config.Product.Name;
                    worksheet.Cells[row, 3].Value = config.ProductSubType.Name;
                }

                // Now read the modified worksheet
                var reader = new ExcelTransactionReader(worksheet, headerRow: 1, detectDataTypes: true);

                var tableName = $"Upload_{configurationId:N}_{DateTime.UtcNow:yyyyMMddHHmmss}";
                var connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                DynamicTableHelper.BulkInsert(connectionString, tableName, reader);

                // Get column info
                var columns = new List<ExcelColumnInfo>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(new ExcelColumnInfo
                    {
                        ColumnName = reader.GetName(i),
                        DataType = reader.GetFieldType(i).Name,
                        ColumnNumber = i + 1
                    });
                }

                response.Success = true;
                response.Message = "Excel uploaded successfully with configuration data";
                response.TableName = tableName;
                response.RowCount = rowCount - 1; // Excluding header
                response.Columns = columns;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading Excel file");

                // Clean up saved file if processing failed
                if (savedFilePath != null && File.Exists(savedFilePath))
                {
                    try
                    {
                        File.Delete(savedFilePath);
                    }
                    catch (Exception deleteEx)
                    {
                        _logger.LogWarning(deleteEx, "Failed to delete file after error: {FilePath}", savedFilePath);
                    }
                }

                response.Success = false;
                response.Message = "Error uploading Excel file";
                response.ValidationErrors = [ex.Message];
                return response;
            }
        }

        /// <summary>
        /// Saves uploaded file to disk for audit/backup purposes
        /// </summary>
        private async Task<string> SaveFileToDiscAsync(IFormFile file)
        {
            // Sanitize filename
            var excelFileName = file.FileName.Trim();
            var sanitizedFileName = excelFileName.Replace(" ", "_");
            var fileName = $"{DateTime.UtcNow.Ticks}_{sanitizedFileName}";

            // Create directory if it doesn't exist
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save file
            var fullPath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("File saved to: {FilePath}", fullPath);
            return fullPath;
        }
    }
}
