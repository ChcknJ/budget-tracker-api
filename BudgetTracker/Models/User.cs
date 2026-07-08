namespace BudgetTracker.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public required string Username { get; set; }
        public required string HashPassword { get; set; }
    }
}
