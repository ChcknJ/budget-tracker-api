namespace BudgetTracker.Models
{
    public class Category
    {
        public Guid Id { get; init; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public required string Name { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
