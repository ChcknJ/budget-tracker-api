using BudgetTracker.DTO;

namespace BudgetTracker.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetResponse?> CreateBudgetAsync (Guid userId, BudgetRequest request);
        Task<BudgetResponse?> EditBudgetAsync (Guid userId, DateOnly month, BudgetRequest request);
        Task<BudgetResponse?> GetBudgetAsync(Guid userId, DateOnly month);
        Task<List<BudgetResponse>> GetAllBudgetsAsync(Guid userId);
    }
}
