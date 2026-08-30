using BudgetTracker.Features.Expenses.DTOs;

namespace BudgetTracker.Features.Expenses.Interfaces
{
    public interface IWriteExpenseService
    {
        Task<ExpenseResult> CreateExpenseAsync(Guid userId, ExpenseRequest request);
        Task<ExpenseResult> EditExpenseAsync(Guid userId, Guid expenseId, ExpenseRequest request);
        Task<ExpenseResult> DeleteExpenseAsync(Guid userId, Guid expenseId);
    }
}
