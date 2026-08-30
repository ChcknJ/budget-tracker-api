using BudgetTracker.Features.Categories.DTOs;
using BudgetTracker.Features.Categories.Interfaces;
using BudgetTracker.Features.Categories.Models;
using BudgetTracker.Features.Expenses.DTOs;
using BudgetTracker.Features.Expenses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> CreateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _writeCategoryService.CreateCategoryAsync(userIdClaim.Value, request, cancellationToken);
            return response.Success ? CreatedAtAction(nameof(GetCategoriesAsync), new { }, response.Response) : MapError(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpPatch("edit-category/{categoryId}")]
        public async Task<IActionResult> EditCategoryAsync(Guid categoryId, CategoryRequest request, CancellationToken cancellationToken)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _writeCategoryService.EditCategoryAsync(userIdClaim.Value, categoryId, request, cancellationToken);
            return response.Success ? Ok(response.Response) : MapError(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _readCategoryService.GetCategoriesAsync(userIdClaim.Value, cancellationToken);
            return Ok(response);
        }


        [EnableRateLimiting("General")]
        [Authorize]
        [HttpDelete("delete-category/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var userIdClaim = GetUserId();

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var response = await _writeCategoryService.DeleteCategoryAsync(userIdClaim.Value, categoryId, cancellationToken);
            return response.Success ? NoContent() : MapError(response);
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

        private IActionResult MapError(CategoryResult result)
        {
            return result.ErrorType switch
            {
                CategoryErrorType.InvalidRequest => BadRequest(result.ErrorMessage),
                CategoryErrorType.CategoryNotFound => NotFound(result.ErrorMessage),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}
