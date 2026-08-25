using BudgetTracker.Features.Auth.Models;
using BudgetTracker.Features.Budgets.Models;
using BudgetTracker.Features.Categories.Models;
using BudgetTracker.Features.Expenses.Models;
using BudgetTracker.Features.Subscriptions.Models;

namespace BudgetTracker.Features.Users.Models
{
    public class User
    {
        public Guid Id { get; init; }

        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string EmailAddress { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
