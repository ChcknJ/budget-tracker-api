using BudgetTracker.Database;
using BudgetTracker.Features.Categories.Models;
using BudgetTracker.Features.Subscriptions.DTOs;
using BudgetTracker.Features.Subscriptions.Interfaces;
using BudgetTracker.Features.Subscriptions.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BudgetTracker.Features.Subscriptions.Services
{
    public class SubscriptionService : IWriteSubscriptionService, IReadSubscriptionService
    {
        private readonly AppDbContext _appDbContext;

        public SubscriptionService(AppDbContext context)
        {
            _appDbContext = context;
        }


        public async Task<SubscriptionResult> CreateSubscriptionAsync (Guid userId, SubscriptionRequest request)
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


            return new SubscriptionResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new SubscriptionResponse(
                    Id:subscription.Id,
                    CategoryId:subscription.CategoryId,
                    Name:subscription.Name,
                    Amount:subscription.Amount,
                    StartDate:subscription.StartDate,
                    EndDate:subscription.EndDate,
                    BillingCycle:subscription.BillingCycle,
                    IsActive:subscription.IsActive
                    ));
        }


        public async Task<SubscriptionResult> EditSubscriptionAsync (Guid userId, Guid subscriptionId, SubscriptionRequest request)
        {
            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == subscriptionId);

            if (subscription==null)
            {
                return new SubscriptionResult(
                Success: true,
                ErrorType: SubscriptionErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            subscription.CategoryId = request.CategoryId;
            subscription.Name = request.Name;
            subscription.Amount = request.Amount;
            subscription.StartDate = request.StartDate;
            subscription.EndDate = request.EndDate;
            subscription.BillingCycle = request.BillingCycle;

            await _appDbContext.SaveChangesAsync();

            return new SubscriptionResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new SubscriptionResponse(
                    Id: subscription.Id,
                    CategoryId: subscription.CategoryId,
                    Name: subscription.Name,
                    Amount: subscription.Amount,
                    StartDate: subscription.StartDate,
                    EndDate: subscription.EndDate,
                    BillingCycle: subscription.BillingCycle,
                    IsActive: subscription.IsActive
                    ));
        }


        public async Task<SubscriptionResult> CancelSubscriptionAsync (Guid userId, Guid subscriptionId)
        {
            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == subscriptionId);

            if (subscription == null)
            {
                return new SubscriptionResult(
                Success: true,
                ErrorType: SubscriptionErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            subscription.IsActive = false;
            await _appDbContext.SaveChangesAsync();

            return new SubscriptionResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new SubscriptionResponse(
                    Id: subscription.Id,
                    CategoryId: subscription.CategoryId,
                    Name: subscription.Name,
                    Amount: subscription.Amount,
                    StartDate: subscription.StartDate,
                    EndDate: subscription.EndDate,
                    BillingCycle: subscription.BillingCycle,
                    IsActive: subscription.IsActive
                    ));
        }


        public async Task<SubscriptionResult> ActivateSubscriptionAsync(Guid userId, Guid subscriptionId)
        {
            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == subscriptionId);

            if (subscription == null)
            {
                return new SubscriptionResult(
                Success: true,
                ErrorType: SubscriptionErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            subscription.IsActive = true;
            await _appDbContext.SaveChangesAsync();

            return new SubscriptionResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new SubscriptionResponse(
                    Id: subscription.Id,
                    CategoryId: subscription.CategoryId,
                    Name: subscription.Name,
                    Amount: subscription.Amount,
                    StartDate: subscription.StartDate,
                    EndDate: subscription.EndDate,
                    BillingCycle: subscription.BillingCycle,
                    IsActive: subscription.IsActive
                    ));
        }

        public async Task<SubscriptionResult> DeleteSubscriptionAsync(Guid userId, Guid subscriptionId)
        {
            var subscription = await _appDbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == subscriptionId);

            if (subscription == null)
            {
                return new SubscriptionResult(
                Success: true,
                ErrorType: SubscriptionErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            subscription.DeletedAt = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();

            return new SubscriptionResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new SubscriptionResponse(
                    Id: subscription.Id,
                    CategoryId: subscription.CategoryId,
                    Name: subscription.Name,
                    Amount: subscription.Amount,
                    StartDate: subscription.StartDate,
                    EndDate: subscription.EndDate,
                    BillingCycle: subscription.BillingCycle,
                    IsActive: subscription.IsActive
                    ));
        }


        public async Task<SubscriptionListResult> GetSubscriptionsAsync (Guid userId)
        {
            int totalSubscription = await _appDbContext.Subscriptions.Where(s => s.UserId == userId).CountAsync();

            var subscriptions = await _appDbContext.Subscriptions.Where(s => s.UserId == userId).Select(s => new SubscriptionResponse(
                    Id: s.Id,
                    CategoryId: s.CategoryId,
                    Name: s.Name,
                    Amount: s.Amount,
                    StartDate: s.StartDate,
                    EndDate: s.EndDate,
                    BillingCycle: s.BillingCycle,
                    IsActive: s.IsActive)).ToListAsync();

            return new SubscriptionListResult (
                TotalSubscription: totalSubscription,
                Subscriptions: subscriptions);
        }
    }
}
