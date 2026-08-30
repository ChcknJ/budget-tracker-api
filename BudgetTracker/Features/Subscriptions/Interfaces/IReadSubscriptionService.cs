using BudgetTracker.Features.Subscriptions.DTOs;

namespace BudgetTracker.Features.Subscriptions.Interfaces
{
    public interface IReadSubscriptionService
    {
        Task<SubscriptionListResult> GetSubscriptionsAsync(Guid userId);

    }
}
