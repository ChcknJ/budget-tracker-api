using BudgetTracker.Features.Subscriptions.DTOs;

namespace BudgetTracker.Features.Subscriptions.Interfaces
{
    public interface IWriteSubscriptionService
    {
        Task<SubscriptionResult> CreateSubscriptionAsync(Guid userId, SubscriptionRequest request);
        Task<SubscriptionResult> EditSubscriptionAsync(Guid userId, Guid subscriptionId, SubscriptionRequest request);
        Task<SubscriptionResult> CancelSubscriptionAsync(Guid userId, Guid subscriptionId);
        Task<SubscriptionResult> ActivateSubscriptionAsync(Guid userId, Guid subscriptionId);
        Task<SubscriptionResult> DeleteSubscriptionAsync(Guid userId, Guid subscriptionId);
    }
}
