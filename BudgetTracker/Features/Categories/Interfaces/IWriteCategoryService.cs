using BudgetTracker.Features.Categories.DTOs;

namespace BudgetTracker.Features.Categories.Interfaces
{
    public interface IWriteCategoryService
    {
        Task<CategoryResult> CreateCategoryAsync(Guid userId, CategoryRequest request);
        Task<CategoryResult> EditCategoryAsync(Guid userId, Guid categoryId, CategoryRequest request);
        Task<CategoryResult> DeleteCategoryAsync(Guid userId, Guid categoryId);
    }
}
