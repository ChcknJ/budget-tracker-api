using BudgetTracker.Features.Expenses.DTOs;

namespace BudgetTracker.Features.Expenses.Interfaces
{
    public interface IWriteExpenseService
    {
        Task<ExpenseResult> CreateExpenseAsync(Guid userId, ExpenseRequest request, CancellationToken cancellationToken);
        Task<ExpenseResult> EditExpenseAsync(Guid userId, Guid expenseId, ExpenseRequest request, CancellationToken cancellationToken);
        Task<ExpenseResult> DeleteExpenseAsync(Guid userId, Guid expenseId, CancellationToken cancellationToken);
    }
}
