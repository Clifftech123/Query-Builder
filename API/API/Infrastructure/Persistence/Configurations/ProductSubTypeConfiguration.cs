using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for ProductSubType entity
    /// </summary>
    public class ProductSubTypeConfiguration : IEntityTypeConfiguration<ProductSubType>
    {
        public void Configure(EntityTypeBuilder<ProductSubType> builder)
        {
            // Table name
            builder.ToTable("ProductSubTypes");

            // Primary key
            builder.HasKey(s => s.Id);

            // Properties
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.UpdatedAt)
                .IsRequired(false);

            builder.Property(s => s.DeletedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(s => s.Name)
                .HasDatabaseName("IX_ProductSubTypes_Name");

            builder.HasIndex(s => s.ProductId)
                .HasDatabaseName("IX_ProductSubTypes_ProductId");

            builder.HasIndex(s => s.DeletedAt)
                .HasDatabaseName("IX_ProductSubTypes_DeletedAt");

            // Query filter for soft delete
            builder.HasQueryFilter(s => s.DeletedAt == null);

            // Relationships
            builder.HasOne(s => s.Product)
                .WithMany(p => p.SubTypes)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
