namespace BudgetTracker.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public required string Username { get; set; }
        public required string HashPassword { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
