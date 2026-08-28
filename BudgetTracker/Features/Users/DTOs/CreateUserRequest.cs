namespace BudgetTracker.Features.Users.DTOs
{
    public record CreateUserRequest(
        string Username, 
        string EmailAddress, 
        string Password);
}
