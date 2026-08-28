using BudgetTracker.Features.Users.Models;

namespace BudgetTracker.Features.Auth.Models
{
    public class RefreshToken
    {
        public Guid Id { get; init; }
        public Guid UserId { get; set; }
        public Guid? ReplacedById { get; set; }

        public required string TokenHash { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }


        // Navigation
        public User User { get; set; } = null!;
        public RefreshToken? ReplacedBy { get; set; }
    }
}
