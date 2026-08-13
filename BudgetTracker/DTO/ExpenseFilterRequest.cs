namespace BudgetTracker.DTO
{
    public class ExpenseFilterRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? SubscriptionId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal? MinimumAmount { get; set; }
        public decimal? MaximumAmount { get; set; }
    }
}
