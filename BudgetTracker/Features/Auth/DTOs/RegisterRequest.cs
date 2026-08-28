namespace BudgetTracker.Features.Auth.DTOs
{
    public record RegisterRequest(
        string Username, 
        string Password, 
        string EmailAddress );
}
