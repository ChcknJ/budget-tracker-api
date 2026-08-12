using BudgetTracker.Database;
using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _appDbContext;

        public SubscriptionService(AppDbContext context)
        {
            _appDbContext = context;
        }


        public async Task<SubscriptionResponse> CreateSubscriptionAsync (Guid userId, SubscriptionRequest request)
        {
            var subscription = new Subscription
            {
                UserId = userId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Amount = request.Amount,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                BillingCycle = request.BillingCycle
            };

            await _appDbContext.Subscriptions.AddAsync(subscription);
            await _appDbContext.SaveChangesAsync();

            return new SubscriptionResponse
            {
                Id = subscription.Id,
                CategoryId = subscription.CategoryId,
                Name = subscription.Name,
                Amount = subscription.Amount,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                BillingCycle = subscription.BillingCycle,
                IsActive = subscription.IsActive
            };
        }


        public async Task<SubscriptionResponse?> EditSubscriptionAsync (Guid userId, Guid subscriptionId, SubscriptionRequest request)
        {
            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == subscriptionId);

            if (subscription==null)
            {
                return null;
            }

            subscription.CategoryId = request.CategoryId;
            subscription.Name = request.Name;
            subscription.Amount = request.Amount;
            subscription.StartDate = request.StartDate;
            subscription.EndDate = request.EndDate;
            subscription.BillingCycle = request.BillingCycle;

            await _appDbContext.SaveChangesAsync();

            return new SubscriptionResponse
            {
                Id = subscription.Id,
                CategoryId = subscription.CategoryId,
                Name = subscription.Name,
                Amount = subscription.Amount,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                BillingCycle = subscription.BillingCycle,
                IsActive = subscription.IsActive
            };
        }


        public async Task<bool> CancelSubscriptionAsync (Guid userId, Guid subscriptionId)
        {
            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == subscriptionId);

            if (subscription == null)
            {
                return false;
            }

            subscription.IsActive = false;
            await _appDbContext.SaveChangesAsync();

            return true;
        }


        public async Task<List<SubscriptionResponse>> GetSubscriptionsAsync (Guid userId)
        {
            List<SubscriptionResponse> subscriptions = await _appDbContext.Subscriptions.Where(s => s.UserId == userId).Select(s=> new SubscriptionResponse
            {
                Id = s.Id,
                CategoryId = s.CategoryId,
                Name = s.Name,
                Amount = s.Amount,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                BillingCycle = s.BillingCycle,
                IsActive = s.IsActive
            }).ToListAsync();

            return subscriptions;
        }
    }
}
