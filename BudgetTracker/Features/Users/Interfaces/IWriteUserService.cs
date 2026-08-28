using BudgetTracker.Features.Users.DTOs;

namespace BudgetTracker.Features.Users.Interfaces
{
    public interface IWriteUserService
    {
        Task<UserResult> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task<UserResult> DeleteUserAsync(Guid userId);
    }
}
