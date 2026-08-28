using BudgetTracker.Features.Users.DTOs;

namespace BudgetTracker.Features.Users.Interfaces
{
    public interface IReadUserService
    {
        Task<UserResult> GetUserAsync(Guid userId); 
    }
}
