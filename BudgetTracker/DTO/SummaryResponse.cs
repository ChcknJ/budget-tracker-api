namespace BudgetTracker.DTO
{
    public class SummaryResponse
    {
        public decimal TotalExpenses { get; set; }
        public int NumberOfExpenses { get; set; }
        public decimal AverageExpense { get; set; }
        public decimal LargestExpense { get; set; }
        public decimal? BudgetForTheMonth { get; set; }
        public decimal? RemainingBudget { get; set; }
        public List<CategoryBreakdownResponse> CategoryBreakdown { get; set; } = [];
    }
}
