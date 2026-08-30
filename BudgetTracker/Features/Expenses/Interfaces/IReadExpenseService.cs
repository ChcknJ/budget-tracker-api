using BudgetTracker.Features.Expenses.DTOs;

namespace BudgetTracker.Features.Expenses.Interfaces
{
    public interface IReadExpenseService
    {
        Task<ExpensePaginatedResult> GetFilteredExpensesAsync(Guid userId, ExpenseQueryRequest query);
    }
}
