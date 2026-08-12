using BudgetTracker.DTO;


namespace BudgetTracker.Interfaces
{
public interface IAuthService
    {
    Task<LoginResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    }
}