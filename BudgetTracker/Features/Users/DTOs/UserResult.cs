using BudgetTracker.Features.Users.Models;

namespace BudgetTracker.Features.Users.DTOs
{
    public record UserResult(
        bool Success,
        UserErrorType? ErrorType,
        string? ErrorMessage,
        UserResponse? Response);
}
