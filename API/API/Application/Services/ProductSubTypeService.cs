using API.Application.Abstractions;
using API.Application.DTOs;
using API.Domain.Models;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services
{
    public class ProductSubTypeService : IProductSubTypeService
    {
        private readonly ApplicationDbContext _context;

        public ProductSubTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSubTypeResponse>> GetAllAsync()
        {
            var subTypes = await _context.ProductSubTypes
                .Include(s => s.Product)
                .ToListAsync();

            return subTypes.Select(s => new ProductSubTypeResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ProductId = s.ProductId,
                ProductName = s.Product.Name,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();
        }

        public async Task<ProductSubTypeResponse?> GetByIdAsync(Guid id)
        {
            var subType = await _context.ProductSubTypes
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subType == null) return null;

            return new ProductSubTypeResponse
            {
                Id = subType.Id,
                Name = subType.Name,
                Description = subType.Description,
                ProductId = subType.ProductId,
                ProductName = subType.Product.Name,
                CreatedAt = subType.CreatedAt,
                UpdatedAt = subType.UpdatedAt
            };
        }

        public async Task<ProductSubTypeResponse> CreateAsync(CreateProductSubTypeRequest request)
        {
            var subType = ProductSubType.Create(request.Name, request.ProductId, request.Description);

            _context.ProductSubTypes.Add(subType);
            await _context.SaveChangesAsync();

            var product = await _context.Products.FindAsync(request.ProductId);

            return new ProductSubTypeResponse
            {
                Id = subType.Id,
                Name = subType.Name,
                Description = subType.Description,
                ProductId = subType.ProductId,
                ProductName = product?.Name ?? "",
                CreatedAt = subType.CreatedAt,
                UpdatedAt = subType.UpdatedAt
            };
        }

        public async Task<ProductSubTypeResponse?> UpdateAsync(Guid id, UpdateProductSubTypeRequest request)
        {
            var subType = await _context.ProductSubTypes
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subType == null) return null;

            subType.Update(request.Name, request.Description);
            await _context.SaveChangesAsync();

            return new ProductSubTypeResponse
            {
                Id = subType.Id,
                Name = subType.Name,
                Description = subType.Description,
                ProductId = subType.ProductId,
                ProductName = subType.Product.Name,
                CreatedAt = subType.CreatedAt,
                UpdatedAt = subType.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var subType = await _context.ProductSubTypes.FindAsync(id);
            if (subType == null) return false;

            subType.Delete();
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
