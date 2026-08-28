using BudgetTracker.Features.Summary.DTOs;

namespace BudgetTracker.Features.Summary.Interfaces
{
    public interface ISummaryService
    {
        Task<SummaryResponse> GetSummaryAsync(Guid userId, DateOnly month);
    }
}
