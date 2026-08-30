using BudgetTracker.Database;
using BudgetTracker.Features.Categories.DTOs;
using BudgetTracker.Features.Categories.Interfaces;
using BudgetTracker.Features.Categories.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BudgetTracker.Features.Categories.Services
{
    public class CategoryService : IReadCategoryService, IWriteCategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }



        public async Task<CategoryResult> CreateCategoryAsync(Guid userId, CategoryRequest request)
        {
            Category category = new Category
            {
                UserId = userId,
                Name = request.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new CategoryResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new CategoryResponse(
                    Id: category.Id,
                    Name: category.Name));
        }


        public async Task<CategoryResult> EditCategoryAsync(Guid userId,Guid categoryId, CategoryRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == categoryId);

            if (category == null)
            {
                return new CategoryResult(
                Success: false,
                ErrorType: CategoryErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            category.Name = request.Name;
            await _context.SaveChangesAsync();

            return new CategoryResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new CategoryResponse(
                    Id: category.Id,
                    Name: category.Name));
        }


        public async Task<CategoryResult> DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
            {
                return new CategoryResult(
                Success: false,
                ErrorType: CategoryErrorType.InvalidRequest,
                ErrorMessage: "Something went wrong.",
                Response: null);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new CategoryResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: null);
        }


        public async Task<CategoryListResult> GetCategoriesAsync(Guid userId)
        {
            var totalCategories = await _context.Categories.Where(category => category.UserId == userId || category.UserId == null).CountAsync();

            var categories = await _context.Categories
                .Where(category => category.UserId == userId || category.UserId == null)
                .Select(category =>
                new CategoryResponse(
                    Id: category.Id,
                    Name: category.Name))
                .ToListAsync();

            return new CategoryListResult(
                TotalCategories: totalCategories,
                Categories: categories);
        }
    }
}
