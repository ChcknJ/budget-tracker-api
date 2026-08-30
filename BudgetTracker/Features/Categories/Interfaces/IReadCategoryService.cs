using BudgetTracker.Features.Categories.DTOs;

namespace BudgetTracker.Features.Categories.Interfaces
{
    public interface IReadCategoryService
    {
        Task<CategoryListResult> GetCategoriesAsync(Guid userId);
    }
}
