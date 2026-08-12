namespace BudgetTracker.DTO
{
    public class ExpenseResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
    }
}
