using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Configuration entity
    /// </summary>
    public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            // Table name
            builder.ToTable("Configurations");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.ConfigurationType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(c => c.TransactionMode)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(c => c.Settings)
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

            builder.Property(c => c.IndustryId)
                .IsRequired();

            builder.Property(c => c.ProductId)
                .IsRequired();

            builder.Property(c => c.ProductSubTypeId)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired(false);

            builder.Property(c => c.DeletedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(c => c.Name)
                .HasDatabaseName("IX_Configurations_Name");

            builder.HasIndex(c => c.IndustryId)
                .HasDatabaseName("IX_Configurations_IndustryId");

            builder.HasIndex(c => c.ProductId)
                .HasDatabaseName("IX_Configurations_ProductId");

            builder.HasIndex(c => c.ProductSubTypeId)
                .HasDatabaseName("IX_Configurations_ProductSubTypeId");

            builder.HasIndex(c => c.DeletedAt)
                .HasDatabaseName("IX_Configurations_DeletedAt");

            // Composite index for common queries
            builder.HasIndex(c => new { c.IndustryId, c.ProductId, c.ProductSubTypeId })
                .HasDatabaseName("IX_Configurations_Hierarchy");

            // Query filter for soft delete
            builder.HasQueryFilter(c => c.DeletedAt == null);

            // Relationships
            builder.HasOne(c => c.Industry)
                .WithMany()
                .HasForeignKey(c => c.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ProductSubType)
                .WithMany()
                .HasForeignKey(c => c.ProductSubTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
