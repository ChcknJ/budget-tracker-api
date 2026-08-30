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


        // !!!! TODO: Race condition between Any and SaveChange (CreateBudget)

        public async Task<BudgetResult> CreateBudgetAsync (Guid userId, BudgetRequest request, CancellationToken cancellationToken)
        {
            var normalizedMonth = NormalizeMonth(request.Month);

            bool isDuplicate = await _context.Budgets.AnyAsync(b => b.UserId == userId && b.Month == normalizedMonth, cancellationToken);

            if (isDuplicate)
            {
                return new BudgetResult(
                    Success: false,
                    ErrorType: BudgetErrorType.InvalidRequest, 
                    ErrorMessage: "Duplicate budget for the month.",
                    Response: null);
            }

            var budget = new Budget
            {
                UserId = userId,
                Month = normalizedMonth,
                Amount = request.Amount
            };

            await _context.Budgets.AddAsync(budget, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new BudgetResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new BudgetResponse(
                    Id: budget.Id,
                    Month: budget.Month,
                    Amount: budget.Amount));
        }

        public async Task<BudgetResult> EditBudgetAsync(Guid userId, DateOnly month, BudgetRequest request, CancellationToken cancellationToken)
        {
            var normalizedMonth = NormalizeMonth(month);

            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.UserId == userId && b.Month == normalizedMonth, cancellationToken);

            if (budget == null)
            {
                return new BudgetResult(
                    Success: false,
                    ErrorType: BudgetErrorType.BudgetNotFound,
                    ErrorMessage: "Budget cannot be found.",
                    Response: null);
            }

            budget.Amount = request.Amount;

            await _context.SaveChangesAsync(cancellationToken);

            return new BudgetResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new BudgetResponse(
                    Id: budget.Id,
                    Month: budget.Month,
                    Amount: budget.Amount));
        }


        public async Task<BudgetResult> GetBudgetAsync (Guid userId, DateOnly month, CancellationToken cancellationToken)
        {
            var normalizedMonth = NormalizeMonth(month);

            var budget = await _context.Budgets.FirstOrDefaultAsync(b =>b.UserId == userId && b.Month == normalizedMonth, cancellationToken);

            if (budget == null)
            {
                return new BudgetResult(
                    Success: false,
                    ErrorType: BudgetErrorType.BudgetNotFound,
                    ErrorMessage: "Budget cannot be found.",
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

        public async Task<BudgetListResult> GetAllBudgetsAsync (Guid userId, CancellationToken cancellationToken)
        {
            var totalBudgets = await _context.Budgets.Where(b => b.UserId == userId).CountAsync(cancellationToken);
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .Select(b => new BudgetResponse(
                Id: b.Id,
                Month: b.Month,
                Amount: b.Amount))
                .ToListAsync(cancellationToken);

            return new BudgetListResult(
                TotalBudgets: totalBudgets,
                Items: budgets);
        }

        private static DateOnly NormalizeMonth(DateOnly month) => new(month.Year, month.Month, 1);
    }
}
