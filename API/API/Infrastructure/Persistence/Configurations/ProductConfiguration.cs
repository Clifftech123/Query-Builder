using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Product entity
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table name
            builder.ToTable("Products");

            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(p => p.IndustryId)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false);

            builder.Property(p => p.DeletedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Products_Name");

            builder.HasIndex(p => p.IndustryId)
                .HasDatabaseName("IX_Products_IndustryId");

            builder.HasIndex(p => p.DeletedAt)
                .HasDatabaseName("IX_Products_DeletedAt");

            // Query filter for soft delete
            builder.HasQueryFilter(p => p.DeletedAt == null);

            // Relationships
            builder.HasOne(p => p.Industry)
                .WithMany(i => i.Products)
                .HasForeignKey(p => p.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.SubTypes)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
