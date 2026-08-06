using BudgetTracker.DTO.Request;


namespace BudgetTracker.Interfaces
{
public interface IAuthService
    {
    Task<bool> RegisterAsync(RegisterRequest registerRequest);
    Task<bool> LoginAsync(LoginRequest loginRequest);

    }
}