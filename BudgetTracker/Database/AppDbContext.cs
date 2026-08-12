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

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    UserId = null,
                    Name = "Food"
                },
                new Category
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    UserId = null,
                    Name = "Transportation"
                },
                new Category
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    UserId = null,
                    Name = "Utilities"
                },
                new Category
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    UserId = null,
                    Name = "Entertainment"
                },
                new Category
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    UserId = null,
                    Name = "Shopping"
                },
                new Category
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    UserId = null,
                    Name = "Health"
                }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
    }
}
