using BudgetTracker.Features.Auth.DTOs;


namespace BudgetTracker.Features.Auth.Interfaces
{
public interface IAuthService
    {
    Task<LoginResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    }
}