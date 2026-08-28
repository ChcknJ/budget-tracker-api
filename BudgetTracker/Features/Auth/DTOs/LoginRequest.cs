namespace BudgetTracker.Features.Auth.DTOs
{
    public record LoginRequest(
        string EmailAddress,
        string Password );
}
