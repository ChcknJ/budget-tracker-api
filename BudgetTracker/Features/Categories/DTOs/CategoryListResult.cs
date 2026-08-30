namespace BudgetTracker.Features.Categories.DTOs
{
    public record CategoryListResult(
        int TotalCategories,
        List<CategoryResponse> Categories);
}
