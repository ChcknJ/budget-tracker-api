namespace BudgetTracker.DTO
{
    public class ExpenseQueryRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? SubscriptionId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public decimal? MinimumAmount { get; set; }
        public decimal? MaximumAmount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "date";
        public string SortOrder { get; set; } = "ascending";
    }
}
