using BudgetTracker.Features.Users.DTOs;

namespace BudgetTracker.Features.Users.Interfaces
{
    public interface IWriteUserService
    {
        Task<UserResult> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken);
        Task<UserResult> DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}
