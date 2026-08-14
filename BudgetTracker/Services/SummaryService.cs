using BudgetTracker.Database;
using BudgetTracker.Interfaces;
using BudgetTracker.DTO;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly AppDbContext _context;

        public SummaryService (AppDbContext context)
        {
            _context = context;
        }

        public async Task<SummaryResponse> GetSummaryAsync(Guid userId, DateOnly month)
        {
            var startDate = new DateOnly(month.Year, month.Month, 1);

            var endDate = startDate.AddMonths(1);

            var expenseQuery = _context.Expenses
                .Where(e =>
                    e.UserId == userId &&
                    e.Date >= startDate &&
                    e.Date < endDate);

            var expenseSummary = await expenseQuery
                .GroupBy(e => 1)
                .Select(g => new
                {
                    TotalExpenses = g.Sum(e => e.Amount),
                    NumberOfExpenses = g.Count(),
                    AverageExpense = g.Average(e => e.Amount),
                    LargestExpense = g.Max(e => e.Amount)
                })
                .FirstOrDefaultAsync();

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b =>
                    b.UserId == userId &&
                    b.Month == startDate);

            var categoryBreakdown = await expenseQuery
                .Join(
                    _context.Categories,
                    expense => expense.CategoryId,
                    category => category.Id,
                    (expense, category) => new
                    {
                        CategoryName = category.Name,
                        expense.Amount
                    })
                .GroupBy(x => x.CategoryName)
                .Select(g => new CategoryBreakdownResponse
                {
                    Name = g.Key,
                    TotalExpense = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            var totalExpenses = expenseSummary?.TotalExpenses ?? 0;
            var numberOfExpenses = expenseSummary?.NumberOfExpenses ?? 0;
            var averageExpense = expenseSummary?.AverageExpense ?? 0;
            var largestExpense = expenseSummary?.LargestExpense ?? 0;

            return new SummaryResponse
            {
                TotalExpenses = totalExpenses,
                NumberOfExpenses = numberOfExpenses,
                AverageExpense = averageExpense,
                LargestExpense = largestExpense,

                BudgetForTheMonth = budget?.Amount,
                RemainingBudget = budget == null
                    ? null
                    : budget.Amount - totalExpenses,

                CategoryBreakdown = categoryBreakdown
            };
        }
    }
}
