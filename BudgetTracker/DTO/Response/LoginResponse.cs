namespace BudgetTracker.DTO.Response
{
    public class LoginResponse
    {
        public bool Success { get; init; }

        public string? Token { get; init; }
        public string TokenType { get; init; } = "Bearer";

        public DateTime? ExpiresAt { get; init; }

        public string? ErrorMessage { get; init; }
    }
}
