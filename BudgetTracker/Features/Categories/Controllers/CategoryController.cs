using BudgetTracker.Features.Categories.DTOs;
using BudgetTracker.Features.Categories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BudgetTracker.Features.Categories.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IReadCategoryService _readCategoryService;
        private readonly IWriteCategoryService _writeCategoryService;

        public CategoryController(IReadCategoryService readCategoryService, IWriteCategoryService writeCategoryService)
        {
            _readCategoryService = readCategoryService;
            _writeCategoryService = writeCategoryService;
        }



        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategoryAsync(CategoryRequest request)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _writeCategoryService.CreateCategoryAsync(userIdClaim.Value, request);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("edit-category/{categoryId}")]
        public async Task<IActionResult> EditCategoryAsync(Guid categoryId, CategoryRequest request)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _writeCategoryService.EditCategoryAsync(userIdClaim.Value, categoryId, request);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _readCategoryService.GetCategoriesAsync(userIdClaim.Value);
            return Ok(response);
        }



        [Authorize]
        [HttpDelete("delete-category/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _writeCategoryService.DeleteCategoryAsync(userIdClaim.Value, categoryId);
            return Ok(response);
        }



        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return null;
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return null;
            }

            return userId;
        }
    }
}
