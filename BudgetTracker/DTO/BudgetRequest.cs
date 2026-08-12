namespace BudgetTracker.DTO
{
    public class BudgetRequest
    {
        public DateOnly Month { get; set; }
        public decimal Amount { get; set; }
    }
}
