using BudgetTracker.Features.Budgets.DTOs;

namespace BudgetTracker.Features.Budgets.Interfaces
{
    public interface IWriteBudgetService
    {
        Task<BudgetResult> CreateBudgetAsync(Guid userId, BudgetRequest request, CancellationToken cancellationToken);
        Task<BudgetResult> EditBudgetAsync(Guid userId, DateOnly month, BudgetRequest request, CancellationToken cancellationToken);
    }
}
