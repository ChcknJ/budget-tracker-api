using BudgetTracker.Features.Subscriptions.Models;

namespace BudgetTracker.Features.Subscriptions.DTOs
{
    public record SubscriptionResponse(
        Guid Id,
        Guid CategoryId,
        string Name,
        decimal Amount,
        DateOnly StartDate,
        DateOnly? EndDate,
        BillingCycle BillingCycle,
        bool IsActive);
}
