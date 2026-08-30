using BudgetTracker.Features.Auth.DTOs;


namespace BudgetTracker.Features.Auth.Interfaces
{
public interface IWriteAuthServices
    {
        Task<AuthResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task<AuthResult> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
        Task<AuthResult> RefreshAsync(RefreshRequest refreshRequest, CancellationToken cancellationToken);
        Task<AuthResult> LogoutAsync(RefreshRequest refreshRequest, CancellationToken cancellationToken);
    }
}