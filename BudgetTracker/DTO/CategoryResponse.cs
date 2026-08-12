using BudgetTracker.Models;

namespace BudgetTracker.DTO
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
