using BudgetTracker.Features.Users.DTOs;
using BudgetTracker.Features.Users.Interfaces;
using BudgetTracker.Features.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BudgetTracker.Features.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWriteUserService _writeUserService;
        private readonly IReadUserService _readUserService;

        public UserController(IWriteUserService writeUserService, IReadUserService readUserService)
        {
            _writeUserService = writeUserService;
            _readUserService = readUserService;
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpGet("get-profile")]
        public async Task<IActionResult> GetUserAsync(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readUserService.GetUserAsync(userId.Value, cancellationToken);

            return response.Success ? Ok(response.Response) : MapError(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeUserService.UpdateUserAsync(userId.Value, request, cancellationToken);

            return response.Success ? Ok(response.Response) : MapError(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }



            var response = await _writeUserService.DeleteUserAsync(userId.Value, cancellationToken);

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


        private IActionResult MapError(UserResult result)
        {
            return result.ErrorType switch
            {
                UserErrorType.UserNotFound => NotFound(result.ErrorMessage),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}
