using API.Application.Abstractions;
using API.Application.DTOs;
using API.Domain.Models;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Industry)
                .ToListAsync();

            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IndustryId = p.IndustryId,
                IndustryName = p.Industry.Name,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Industry)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                IndustryId = product.IndustryId,
                IndustryName = product.Industry.Name,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var product = Product.Create(request.Name, request.IndustryId, request.Description);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var industry = await _context.Industries.FindAsync(request.IndustryId);

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                IndustryId = product.IndustryId,
                IndustryName = industry?.Name ?? "",
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _context.Products
                .Include(p => p.Industry)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            product.Update(request.Name, request.Description);
            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                IndustryId = product.IndustryId,
                IndustryName = product.Industry.Name,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Delete();
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
