using BudgetTracker.Database;
using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ExpenseResponse> CreateExpenseAsync (ExpenseRequest request, Guid userId)
        {
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

            ExpenseResponse response = new ExpenseResponse
            {
                Id = expense.Id,
                CategoryId = expense.CategoryId,
                SubscriptionId = expense.SubscriptionId,
                Amount = expense.Amount,
                Description = expense.Description,
                Date = expense.Date
            };

            return response;
        }


        public async Task<ExpenseResponse?> EditExpenseAsync (Guid userId, Guid expenseId, ExpenseRequest request)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                return null;
            }

            expense.CategoryId = request.CategoryId;
            expense.SubscriptionId = request.SubscriptionId;
            expense.Amount = request.Amount;
            expense.Description = request.Description;
            expense.Date = request.Date;

            await _context.SaveChangesAsync();

            return new ExpenseResponse
            {
                Id = expenseId,
                CategoryId = expense.CategoryId,
                SubscriptionId = expense.SubscriptionId,
                Amount = expense.Amount,
                Description = expense.Description,
                Date = expense.Date
            };
        }

        public async Task<bool> DeleteExpenseAsync (Guid userId, Guid expenseId)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(expense => expense.Id == expenseId && expense.UserId == userId);

            if (expense == null)
            {
                return false;
            }
        
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ExpenseResponse>> GetExpensesAsync (Guid userId, ExpenseFilterRequest filter)
        {
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

            var response = await query.Select(e => new ExpenseResponse
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                SubscriptionId = e.SubscriptionId,
                Amount = e.Amount,
                Description = e.Description,
                Date = e.Date
            }).ToListAsync();

            return response;
        }
    }
}
