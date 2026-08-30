using BudgetTracker.Features.Subscriptions.Models;

namespace BudgetTracker.Features.Subscriptions.DTOs
{
    public record SubscriptionRequest(
        Guid CategoryId,
        string Name,
        decimal Amount,
        DateOnly StartDate,
        DateOnly? EndDate,
        BillingCycle BillingCycle);
}
