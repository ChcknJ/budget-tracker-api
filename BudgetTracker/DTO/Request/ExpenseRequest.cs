namespace BudgetTracker.DTO.Request
{
    public class ExpenseRequest
    {
        public Guid CategoryId { get; set; }
        public Guid? SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
    }
}
