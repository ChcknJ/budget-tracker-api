using BudgetTracker.Features.Auth.Models;

namespace BudgetTracker.Features.Auth.DTOs
{
    public record AuthResult(
        bool Success,
        AuthErrorType? ErrorType,
        string? ErrorMessage,
        AuthResponse? Response);
}
