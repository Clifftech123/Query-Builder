using API.Application.Abstractions;
using API.Application.DTOs;
using API.Domain.Models;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConfigurationResponse>> GetAllAsync()
        {
            var configurations = await _context.Configurations
                .Include(c => c.Industry)
                .Include(c => c.Product)
                .Include(c => c.ProductSubType)
                .ToListAsync();

            return configurations.Select(c => new ConfigurationResponse
            {
                Id = c.Id,
                ConfigurationType = c.ConfigurationType,
                Name = c.Name,
                TransactionMode = c.TransactionMode,
                IndustryId = c.IndustryId,
                IndustryName = c.Industry.Name,
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                ProductSubTypeId = c.ProductSubTypeId,
                ProductSubTypeName = c.ProductSubType.Name,
                Settings = c.Settings,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        public async Task<ConfigurationResponse?> GetByIdAsync(Guid id)
        {
            var config = await _context.Configurations
                .Include(c => c.Industry)
                .Include(c => c.Product)
                .Include(c => c.ProductSubType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (config == null) return null;

            return new ConfigurationResponse
            {
                Id = config.Id,
                ConfigurationType = config.ConfigurationType,
                Name = config.Name,
                TransactionMode = config.TransactionMode,
                IndustryId = config.IndustryId,
                IndustryName = config.Industry.Name,
                ProductId = config.ProductId,
                ProductName = config.Product.Name,
                ProductSubTypeId = config.ProductSubTypeId,
                ProductSubTypeName = config.ProductSubType.Name,
                Settings = config.Settings,
                CreatedAt = config.CreatedAt,
                UpdatedAt = config.UpdatedAt
            };
        }

        public async Task<ConfigurationResponse> CreateAsync(CreateConfigurationRequest request)
        {
            var config = Configuration.Create(
                request.ConfigurationType,
                request.Name,
                request.TransactionMode,
                request.IndustryId,
                request.ProductId,
                request.ProductSubTypeId,
                request.Settings
            );

            _context.Configurations.Add(config);
            await _context.SaveChangesAsync();

            var industry = await _context.Industries.FindAsync(request.IndustryId);
            var product = await _context.Products.FindAsync(request.ProductId);
            var subType = await _context.ProductSubTypes.FindAsync(request.ProductSubTypeId);

            return new ConfigurationResponse
            {
                Id = config.Id,
                ConfigurationType = config.ConfigurationType,
                Name = config.Name,
                TransactionMode = config.TransactionMode,
                IndustryId = config.IndustryId,
                IndustryName = industry?.Name ?? "",
                ProductId = config.ProductId,
                ProductName = product?.Name ?? "",
                ProductSubTypeId = config.ProductSubTypeId,
                ProductSubTypeName = subType?.Name ?? "",
                Settings = config.Settings,
                CreatedAt = config.CreatedAt,
                UpdatedAt = config.UpdatedAt
            };
        }

        public async Task<ConfigurationResponse?> UpdateAsync(Guid id, UpdateConfigurationRequest request)
        {
            var config = await _context.Configurations
                .Include(c => c.Industry)
                .Include(c => c.Product)
                .Include(c => c.ProductSubType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (config == null) return null;

            config.Update(request.ConfigurationType, request.Name, request.TransactionMode, request.Settings);
            await _context.SaveChangesAsync();

            return new ConfigurationResponse
            {
                Id = config.Id,
                ConfigurationType = config.ConfigurationType,
                Name = config.Name,
                TransactionMode = config.TransactionMode,
                IndustryId = config.IndustryId,
                IndustryName = config.Industry.Name,
                ProductId = config.ProductId,
                ProductName = config.Product.Name,
                ProductSubTypeId = config.ProductSubTypeId,
                ProductSubTypeName = config.ProductSubType.Name,
                Settings = config.Settings,
                CreatedAt = config.CreatedAt,
                UpdatedAt = config.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var config = await _context.Configurations.FindAsync(id);
            if (config == null) return false;

            config.Delete();
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
