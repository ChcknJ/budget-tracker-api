using BudgetTracker.Features.Subscriptions.Models;

namespace BudgetTracker.Features.Subscriptions.DTOs
{
    public class SubscriptionRequest
    {
        public Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public required BillingCycle BillingCycle { get; set; }
    }
}
