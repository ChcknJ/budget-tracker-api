using BudgetTracker.Features.Budgets.DTOs;

namespace BudgetTracker.Features.Budgets.Interfaces
{
    public interface IReadBudgetService
    {
        Task<BudgetListResult> GetAllBudgetsAsync(Guid userId, CancellationToken cancellationToken);
        Task<BudgetResult> GetBudgetAsync(Guid userId, DateOnly month, CancellationToken cancellationToken);

    }
}
