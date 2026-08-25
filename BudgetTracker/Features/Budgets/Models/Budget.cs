using BudgetTracker.Features.Users.Models;

namespace BudgetTracker.Features.Budgets.Models
{
    public class Budget
    {
        public Guid Id { get; init; }
        public Guid UserId { get; set; }

        public DateOnly Month { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation
        public User User { get; set; } = null!;
    }
}
