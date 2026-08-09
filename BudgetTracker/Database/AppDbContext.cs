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

            modelBuilder.Entity<User>().Property(user => user.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Category>().Property(category => category.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Expense>().Property(expense => expense.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Subscription>().Property(subscription => subscription.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Budget>().Property(budget => budget.Id).HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Subscription)
                .WithMany(s => s.Expenses)
                .HasForeignKey(e => e.SubscriptionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Budget>()
                .HasIndex(b => new { b.UserId, b.Month })
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
    }
}
