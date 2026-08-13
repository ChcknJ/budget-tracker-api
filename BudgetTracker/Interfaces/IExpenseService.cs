using BudgetTracker.DTO;
namespace BudgetTracker.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseResponse> CreateExpenseAsync (ExpenseRequest request, Guid userId);
        Task<ExpenseResponse?> EditExpenseAsync(Guid userId, Guid expenseId, ExpenseRequest request);
        Task<bool> DeleteExpenseAsync (Guid userId, Guid expenseId);
        Task<List<ExpenseResponse>> GetExpensesAsync(Guid userId, ExpenseFilterRequest filter);
    }
}
