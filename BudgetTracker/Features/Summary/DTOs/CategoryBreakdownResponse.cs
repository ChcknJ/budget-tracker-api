using System.Runtime.CompilerServices;

namespace BudgetTracker.Features.Summary.DTOs
{
    public record CategoryBreakdownResponse(
        string Name,
        decimal TotalExpense);
}
