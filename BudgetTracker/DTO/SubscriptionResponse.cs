namespace BudgetTracker.DTO
{
    public class SubscriptionResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public required string BillingCycle { get; set; } = "Monthly";
        public bool IsActive { get; set; }
    }
}
