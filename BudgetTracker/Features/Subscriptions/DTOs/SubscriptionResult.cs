using BudgetTracker.Features.Subscriptions.Models;

namespace BudgetTracker.Features.Subscriptions.DTOs
{
    public record SubscriptionResult(
        bool Success,
        SubscriptionErrorType? ErrorType,
        string? ErrorMessage,
        SubscriptionResponse? Response);
}
