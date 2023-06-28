using Cache.API.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cache.API.Persistence
{
    public class TestDbContext : DbContext
    {
        public virtual DbSet<TestEntity> Users { get; set; }

        protected TestDbContext() { }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).Assembly);
        }
    }
}
