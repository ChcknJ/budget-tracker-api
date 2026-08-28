namespace BudgetTracker.Features.Users.DTOs
{
    public record UpdateUserRequest(
        string Username, 
        string EmailAddress, 
        string Password);
}
