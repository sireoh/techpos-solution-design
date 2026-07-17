using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;

namespace Onion.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // where Onion.Infrastructure.Data.Configurations.OrderConfigurations.Configure() is called
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
