using BudgetTracker.DTO;

namespace BudgetTracker.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionResponse> CreateSubscriptionAsync(Guid userId, SubscriptionRequest request);
        Task<SubscriptionResponse?> EditSubscriptionAsync(Guid userId, Guid subscriptionId, SubscriptionRequest request);
        Task<bool> CancelSubscriptionAsync(Guid userId, Guid subscriptionId);
        Task<List<SubscriptionResponse>> GetSubscriptionsAsync(Guid userId);
    }
}
