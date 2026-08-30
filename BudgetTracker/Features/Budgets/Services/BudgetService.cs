using BudgetTracker.Database;
using BudgetTracker.Features.Budgets.DTOs;
using BudgetTracker.Features.Budgets.Interfaces;
using BudgetTracker.Features.Budgets.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BudgetTracker.Features.Budgets.Services
{
    public class BudgetService : IReadBudgetService, IWriteBudgetService
    {
        private readonly AppDbContext _context;

        public BudgetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BudgetResult> CreateBudgetAsync (Guid userId, BudgetRequest request)
        {
            // check duplicate
            bool isDuplicate = await _context.Budgets.AnyAsync(b => b.UserId == userId && b.Month == request.Month);

            if (isDuplicate)
            {
                return new BudgetResult(
                    Success: false,
                    ErrorType: BudgetErrorType.InvalidRequest,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            var budget = new Budget
            {
                UserId = userId,
                Month = request.Month,
                Amount = request.Amount
            };

            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();

            return new BudgetResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new BudgetResponse(
                    Id: budget.Id,
                    Month: budget.Month,
                    Amount: budget.Amount));
        }

        public async Task<BudgetResult> EditBudgetAsync(Guid userId, DateOnly month, BudgetRequest request)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.UserId == userId && b.Month == month);

            if (budget == null)
            {
                return new BudgetResult(
                    Success: false,
                    ErrorType: BudgetErrorType.InvalidRequest,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            budget.Amount = request.Amount;

            await _context.SaveChangesAsync();

            return new BudgetResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new BudgetResponse(
                    Id: budget.Id,
                    Month: budget.Month,
                    Amount: budget.Amount));
        }


        public async Task<BudgetResult> GetBudgetAsync (Guid userId, DateOnly month)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(b =>b.UserId == userId && b.Month == month);

            if (budget == null)
            {
                return new BudgetResult(
                    Success: false,
                    ErrorType: BudgetErrorType.InvalidRequest,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            return new BudgetResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new BudgetResponse(
                    Id: budget.Id,
                    Month: budget.Month,
                    Amount: budget.Amount));
        }

        public async Task<BudgetListResult> GetAllBudgetsAsync (Guid userId)
        {
            var totalBudgets = await _context.Budgets.Where(b => b.UserId == userId).CountAsync();
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .Select(b => new BudgetResponse(
                Id: b.Id,
                Month: b.Month,
                Amount: b.Amount))
                .ToListAsync();

            return new BudgetListResult(
                TotalBudgets: totalBudgets,
                Items: budgets);
        }
    }
}
