namespace BudgetTracker.Features.Budgets.DTOs
{
    public class BudgetResponse
    {
        public Guid Id { get; set; }
        public DateOnly Month { get; set; }
        public decimal Amount { get; set; }
    }
}
