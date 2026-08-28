using System.Runtime.CompilerServices;

namespace BudgetTracker.Features.Summary.DTOs
{
    public class CategoryBreakdownResponse
    {
        public required string Name { get; set; }
        public decimal TotalExpense { get; set; }
    }
}
