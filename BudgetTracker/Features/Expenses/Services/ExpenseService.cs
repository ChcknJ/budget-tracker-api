using BudgetTracker.Database;
using BudgetTracker.Features.Categories.Models;
using BudgetTracker.Features.Expenses.DTOs;
using BudgetTracker.Features.Expenses.Interfaces;
using BudgetTracker.Features.Expenses.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Features.Expenses.Services
{
    public class ExpenseService : IWriteExpenseService, IReadExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ExpenseResult> CreateExpenseAsync(Guid userId, ExpenseRequest request)
        {
            bool isValidCategory = await ValidateUserCategory(userId, request.CategoryId);
            if (!isValidCategory)
            {
                return new ExpenseResult(
                Success: false,
                ErrorType: ExpenseErrorType.InvalidCategory,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            if (request.SubscriptionId.HasValue)
            {
                bool isValidSubscription = await ValidateUserSubscription(userId, request.SubscriptionId);
                if (!isValidSubscription)
                {
                    return new ExpenseResult(
                    Success: false,
                    ErrorType: ExpenseErrorType.InvalidSubsription,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
                }
            }


            Expense expense = new Expense
            {
                UserId = userId,
                CategoryId = request.CategoryId,
                SubscriptionId = request.SubscriptionId,
                Amount = request.Amount,
                Description = request.Description,
                Date = request.Date
            };

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return new ExpenseResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new ExpenseResponse(
                    Id: expense.Id,
                    CategoryId: expense.CategoryId,
                    SubscriptionId: expense.SubscriptionId,
                    Amount: expense.Amount,
                    Description: expense.Description,
                    Date: expense.Date));
        }

        public async Task<ExpenseResult> EditExpenseAsync (Guid userId, Guid expenseId, ExpenseRequest request)
        {
            bool isValidCategory = await ValidateUserCategory(userId, request.CategoryId);
            if (!isValidCategory)
            {
                return new ExpenseResult(
                Success: false,
                ErrorType: ExpenseErrorType.InvalidCategory,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            if (request.SubscriptionId.HasValue)
            {
                bool isValidSubscription = await ValidateUserSubscription(userId, request.SubscriptionId);
                if (!isValidSubscription)
                {
                    return new ExpenseResult(
                    Success: false,
                    ErrorType: ExpenseErrorType.InvalidSubsription,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
                }
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                return new ExpenseResult(
                Success: false,
                ErrorType: ExpenseErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            expense.CategoryId = request.CategoryId;
            expense.SubscriptionId = request.SubscriptionId;
            expense.Amount = request.Amount;
            expense.Description = request.Description;
            expense.Date = request.Date;

            await _context.SaveChangesAsync();

            return new ExpenseResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new ExpenseResponse(
                    Id: expense.Id,
                    CategoryId: expense.CategoryId,
                    SubscriptionId: expense.SubscriptionId,
                    Amount: expense.Amount,
                    Description: expense.Description,
                    Date: expense.Date));
        }

        public async Task<ExpenseResult> DeleteExpenseAsync (Guid userId, Guid expenseId)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(expense => expense.Id == expenseId && expense.UserId == userId);

            if (expense == null)
            {
                return new ExpenseResult(
                    Success: false,
                    ErrorType: ExpenseErrorType.InvalidRequest,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            expense.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new ExpenseResult(
                 Success: true,
                 ErrorType: null,
                 ErrorMessage: null,
                 Response: null);
        }

        public async Task<ExpensePaginatedResult> GetFilteredExpensesAsync (Guid userId, ExpenseQueryRequest filter)
        {
            // Filters

            var query =  _context.Expenses.Where(expense => expense.UserId == userId);


            if (filter.CategoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == filter.CategoryId);
            }

            if (filter.SubscriptionId.HasValue)
            {
                query = query.Where(e => e.SubscriptionId == filter.SubscriptionId);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(e => e.Date >= filter.FromDate);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(e => e.Date <= filter.ToDate);
            }

            if (filter.MinimumAmount.HasValue)
            {
                query = query.Where(e => e.Amount >= filter.MinimumAmount);
            }

            if (filter.MaximumAmount.HasValue)
            {
                query = query.Where(e => e.Amount <= filter.MaximumAmount);
            }


            var totalItems = await query.CountAsync();


            var sortBy = filter.SortByRequest;
            var sortOrder = filter.SortingOrder;

            query = (sortBy, sortOrder) switch
            {
                (SortBy.Date, SortOrder.Ascending) =>
                    query.OrderBy(e => e.Date),

                (SortBy.Date, SortOrder.Descending) =>
                    query.OrderByDescending(e => e.Date),

                (SortBy.Amount, SortOrder.Ascending) =>
                    query.OrderBy(e => e.Amount),

                (SortBy.Amount, SortOrder.Descending) =>
                    query.OrderByDescending(e => e.Amount),

                _ => query.OrderByDescending(e => e.Date)
            };





            // Pagination
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            query = query.Skip(skip).Take(filter.PageSize);
    

            var items = await query.Select(e => new ExpenseResponse(
                Id:e.Id,
                CategoryId:e.CategoryId,
                SubscriptionId:e.SubscriptionId,
                Amount:e.Amount,
                Description:e.Description,
                Date:e.Date)).ToListAsync();



            return new ExpensePaginatedResult(
                Page: filter.PageNumber,
                PageSize: filter.PageSize,
                TotalItems: totalItems,
                TotalPages: (int)Math.Ceiling((double)totalItems / filter.PageSize),
                Items: items);
        }










        private async Task<bool> ValidateUserCategory(Guid userId, Guid categoryId)
        {
            // Validate Category userId==null Categories are universal category
            var validCategory = await _context.Categories.AnyAsync(c => c.Id == categoryId && c.UserId == userId || c.UserId == null);
            return validCategory;
        }

        private async Task<bool> ValidateUserSubscription(Guid userId, Guid? subscriptionId)
        {
            var validSubscription = await _context.Categories.AnyAsync(s => s.Id == subscriptionId && s.UserId == userId);
            return validSubscription;
        }
    }
}
