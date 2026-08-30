using BudgetTracker.Features.Expenses.Models;
using BudgetTracker.Features.Subscriptions.Models;
using BudgetTracker.Features.Users.Models;

namespace BudgetTracker.Features.Categories.Models
{
    public class Category
    {
        public Guid Id { get; init; }
        public Guid? UserId { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User? User { get; set; }
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
