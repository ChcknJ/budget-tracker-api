using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategoryAsync(CategoryRequest request)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _categoryService.CreateCategoryAsync(userIdClaim.Value, request);
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("edit-category/{categoryId}")]
        public async Task<IActionResult> EditCategoryAsync(Guid categoryId, CategoryRequest request)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _categoryService.EditCategoryAsync(userIdClaim.Value, categoryId, request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _categoryService.GetCategoriesAsync(userIdClaim.Value);
            
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

            var response = await _categoryService.DeleteCategoryAsync(userIdClaim.Value, categoryId);
            if (!response)
            {
                return NotFound();
            }
            return NoContent();
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
