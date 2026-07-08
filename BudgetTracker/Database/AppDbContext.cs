using Microsoft.EntityFrameworkCore;
using BudgetTracker.Models;

namespace BudgetTracker.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");
            
        }

        public DbSet<User> Users { get; set; }
    }
}
