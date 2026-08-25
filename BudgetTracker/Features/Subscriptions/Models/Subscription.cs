using BudgetTracker.Features.Categories.Models;
using BudgetTracker.Features.Expenses.Models;
using BudgetTracker.Features.Users.Models;

namespace BudgetTracker.Features.Subscriptions.Models
{
    public class Subscription
    {
        public Guid Id { get; init; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }

        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public required BillingCycle BillingCycle { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        // Navigation
        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
