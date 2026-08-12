namespace BudgetTracker.Models
{
    public class Budget
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Month { get; set; }
        public decimal Amount { get; set; }

        public User User { get; set; } = null!;
    }
}
