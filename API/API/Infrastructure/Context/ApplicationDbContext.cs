using API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Context
{
    /// <summary>
    /// Application database context
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Industry> Industries => Set<Industry>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductSubType> ProductSubTypes => Set<ProductSubType>();
        public DbSet<Configuration> Configurations => Set<Configuration>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }


    }
}

