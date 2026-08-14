using BudgetTracker.DTO;

namespace BudgetTracker.Interfaces
{
    public interface ISummaryService
    {
        Task<SummaryResponse> GetSummaryAsync(Guid userId, DateOnly month);
    }
}
