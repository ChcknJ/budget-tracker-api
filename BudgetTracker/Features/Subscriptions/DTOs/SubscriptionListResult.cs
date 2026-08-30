namespace BudgetTracker.Features.Subscriptions.DTOs
{
    public record SubscriptionListResult(
        int TotalSubscription,
        List<SubscriptionResponse> Subscriptions);
}
