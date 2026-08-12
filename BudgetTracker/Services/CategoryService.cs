using BudgetTracker.Database;
using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponse> CreateCategoryAsync(Guid userId, CategoryRequest request)
        {
            Category category = new Category
            {
                UserId = userId,
                Name = request.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }


        public async Task<CategoryResponse?> EditCategoryAsync(Guid userId,Guid categoryId, CategoryRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == categoryId);

            if (category == null)
            {
                return null;
            }

            category.Name = request.Name;

            await _context.SaveChangesAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }


        public async Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<CategoryResponse>> GetCategoriesAsync(Guid userId)
        {
            var categories = await _context.Categories.Where(category => category.UserId == userId || category.UserId == null).Select(category =>
                new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name
                }
                ).ToListAsync();

            return categories;
        }
    }
}
