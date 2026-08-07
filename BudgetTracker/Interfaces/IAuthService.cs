using BudgetTracker.DTO.Request;
using BudgetTracker.DTO.Response;


namespace BudgetTracker.Interfaces
{
public interface IAuthService
    {
    Task<LoginResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    }
}