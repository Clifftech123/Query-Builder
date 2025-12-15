using API.Application.Abstractions;
using API.Application.DTOs;
using API.Domain.Models;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services
{
    public class IndustryService : IIndustryService
    {
        private readonly ApplicationDbContext _context;

        public IndustryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IndustryResponse>> GetAllAsync()
        {
            var industries = await _context.Industries.ToListAsync();

            return industries.Select(i => new IndustryResponse
            {
                Id = i.Id,
                Name = i.Name,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList();
        }

        public async Task<IndustryResponse?> GetByIdAsync(Guid id)
        {
            var industry = await _context.Industries.FindAsync(id);
            if (industry == null) return null;

            return new IndustryResponse
            {
                Id = industry.Id,
                Name = industry.Name,
                CreatedAt = industry.CreatedAt,
                UpdatedAt = industry.UpdatedAt
            };
        }

        public async Task<IndustryResponse> CreateAsync(CreateIndustryRequest request)
        {
            var industry = Industry.Create(request.Name);

            _context.Industries.Add(industry);
            await _context.SaveChangesAsync();

            return new IndustryResponse
            {
                Id = industry.Id,
                Name = industry.Name,
                CreatedAt = industry.CreatedAt,
                UpdatedAt = industry.UpdatedAt
            };
        }

        public async Task<IndustryResponse?> UpdateAsync(Guid id, UpdateIndustryRequest request)
        {
            var industry = await _context.Industries.FindAsync(id);
            if (industry == null) return null;

            industry.Update(request.Name);
            await _context.SaveChangesAsync();

            return new IndustryResponse
            {
                Id = industry.Id,
                Name = industry.Name,
                CreatedAt = industry.CreatedAt,
                UpdatedAt = industry.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var industry = await _context.Industries.FindAsync(id);
            if (industry == null) return false;

            industry.Delete();
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
