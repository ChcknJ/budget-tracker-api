using BudgetTracker.Features.Budgets.DTOs;

namespace BudgetTracker.Features.Budgets.Interfaces
{
    public interface IWriteBudgetService
    {
        Task<BudgetResult> CreateBudgetAsync(Guid userId, BudgetRequest request);
        Task<BudgetResult> EditBudgetAsync(Guid userId, DateOnly month, BudgetRequest request);
    }
}
