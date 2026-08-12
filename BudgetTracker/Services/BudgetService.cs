using BudgetTracker.Database;
using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BudgetTracker.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly AppDbContext _context;

        public BudgetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BudgetResponse?> CreateBudgetAsync (Guid userId, BudgetRequest request)
        {
            // check duplicate
            bool isDuplicate = await _context.Budgets.AnyAsync(b => b.UserId == userId && b.Month == request.Month);

            if (isDuplicate)
            {   
                return null;
            }

            var budget = new Budget
            {
                UserId = userId,
                Month = request.Month,
                Amount = request.Amount
            };

            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();

            return new BudgetResponse
            {
                Id = budget.Id,
                Month = budget.Month,
                Amount = budget.Amount
            };
        }

        public async Task<BudgetResponse?> EditBudgetAsync(Guid userId, DateOnly month, BudgetRequest request)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.UserId == userId &&b.Month == month);

            if (budget == null)
            {
                return null;
            }

            budget.Amount = request.Amount;

            await _context.SaveChangesAsync();

            return new BudgetResponse
            {
                Id = budget.Id,
                Month = budget.Month,
                Amount = budget.Amount
            };
        }


        public async Task<BudgetResponse?> GetBudgetAsync (Guid userId, DateOnly month)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(b =>b.UserId == userId && b.Month == month);

            if (budget == null)
            {
                return null;
            }

            return new BudgetResponse
            {
                Id = budget.Id,
                Month = budget.Month,
                Amount = budget.Amount
            };
        }

        public async Task<List<BudgetResponse>> GetAllBudgetsAsync (Guid userId)
        {
            var budgets = await _context.Budgets.Where(b => b.UserId == userId).Select(b => new BudgetResponse
            {
                Id = b.Id,
                Month = b.Month,
                Amount = b.Amount
            }).ToListAsync();

            return budgets;
        }
    }
}
