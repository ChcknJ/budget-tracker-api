namespace BudgetTracker.DTO.Response
{
    public class RegisterResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
