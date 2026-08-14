using System.Runtime.CompilerServices;

namespace BudgetTracker.DTO
{
    public class CategoryBreakdownResponse
    {
        public required string Name { get; set; }
        public decimal TotalExpense { get; set; }
    }
}
