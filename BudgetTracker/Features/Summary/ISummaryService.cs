namespace BudgetTracker.Features.Summary
{
    public interface ISummaryService
    {
        Task<SummaryResponse> GetSummaryAsync(Guid userId, DateOnly month);
    }
}
