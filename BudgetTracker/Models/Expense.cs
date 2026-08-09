namespace BudgetTracker.Models
{
    public class Expense
    {
        public Guid Id { get; init; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }

        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public Subscription? Subscription { get; set; }
    }
}
