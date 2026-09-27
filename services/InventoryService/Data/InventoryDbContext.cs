using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(
            DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<SparePart> SpareParts => Set<SparePart>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SparePart>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Description).IsRequired().HasMaxLength(1000);
                entity.Property(x => x.Quantity).IsRequired();
                entity.Property(x => x.UnitPrice).HasPrecision(18, 2).IsRequired();
                entity.HasIndex(x => x.Name);
            });
        }
    }
}
