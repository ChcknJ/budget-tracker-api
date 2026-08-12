namespace BudgetTracker.DTO
{
    public class RegisterResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
