using BudgetTracker.DTO;

namespace BudgetTracker.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategoryAsync(Guid userId, CategoryRequest request);
        Task<CategoryResponse?> EditCategoryAsync(Guid userId, Guid categoryId, CategoryRequest request);
        Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId);
        Task<List<CategoryResponse>> GetCategoriesAsync(Guid userId);
    }
}
