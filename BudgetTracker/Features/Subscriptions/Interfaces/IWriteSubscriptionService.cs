using BudgetTracker.Features.Subscriptions.DTOs;

namespace BudgetTracker.Features.Subscriptions.Interfaces
{
    public interface IWriteSubscriptionService
    {
        Task<SubscriptionResult> CreateSubscriptionAsync(Guid userId, SubscriptionRequest request, CancellationToken cancellationToken);
        Task<SubscriptionResult> EditSubscriptionAsync(Guid userId, Guid subscriptionId, SubscriptionRequest request, CancellationToken cancellationToken);
        Task<SubscriptionResult> CancelSubscriptionAsync(Guid userId, Guid subscriptionId, CancellationToken cancellationToken);
        Task<SubscriptionResult> ActivateSubscriptionAsync(Guid userId, Guid subscriptionId, CancellationToken cancellationToken);
        Task<SubscriptionResult> DeleteSubscriptionAsync(Guid userId, Guid subscriptionId, CancellationToken cancellationToken);
    }
}
