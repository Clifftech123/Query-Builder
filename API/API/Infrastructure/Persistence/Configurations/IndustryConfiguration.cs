using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Industry entity
    /// </summary>
    public class IndustryConfiguration : IEntityTypeConfiguration<Industry>
    {
        public void Configure(EntityTypeBuilder<Industry> builder)
        {
            // Table name
            builder.ToTable("Industries");

            // Primary key
            builder.HasKey(i => i.Id);

            // Properties
            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.Property(i => i.UpdatedAt)
                .IsRequired(false);

            builder.Property(i => i.DeletedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(i => i.Name)
                .HasDatabaseName("IX_Industries_Name");

            builder.HasIndex(i => i.DeletedAt)
                .HasDatabaseName("IX_Industries_DeletedAt");

            // Query filter for soft delete
            builder.HasQueryFilter(i => i.DeletedAt == null);

            // Relationships
            builder.HasMany(i => i.Products)
                .WithOne(p => p.Industry)
                .HasForeignKey(p => p.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
