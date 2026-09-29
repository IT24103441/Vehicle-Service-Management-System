using Microsoft.EntityFrameworkCore;
using BillingService.Models;

namespace BillingService.Data
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(
            DbContextOptions<BillingDbContext> options)
            : base(options)
        {
        }
        public DbSet<PartCharge> PartCharges => Set<PartCharge>();
        public DbSet<ProcessedKafkaEvent> ProcessedKafkaEvents => Set<ProcessedKafkaEvent>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCharge>(entity => { entity.HasKey(x => x.Id); entity.Property(x => x.SparePartName).IsRequired().HasMaxLength(200); entity.Property(x => x.UnitPrice).HasPrecision(18, 2); entity.Property(x => x.TotalAmount).HasPrecision(18, 2); entity.HasIndex(x => x.PartIssueId).IsUnique(); entity.HasIndex(x => x.JobCardId); });
            modelBuilder.Entity<ProcessedKafkaEvent>(entity => { entity.HasKey(x => x.Id); entity.Property(x => x.EventType).IsRequired().HasMaxLength(100); entity.HasIndex(x => x.EventId).IsUnique(); });
        }
    }
}
