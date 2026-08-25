
namespace BudgetTracker.Features.Categories.DTOs
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
