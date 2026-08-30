namespace BudgetTracker.Features.Auth.DTOs
{
    public record AuthResponse(
        string AccessToken,
        string RefreshToken );
}
