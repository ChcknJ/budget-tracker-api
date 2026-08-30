namespace BudgetTracker.Features.Expenses.DTOs
{
    public record ExpenseResponse(
        Guid Id,
        Guid CategoryId,
        Guid? SubscriptionId,
        decimal Amount,
        string? Description,
        DateOnly Date);
}
