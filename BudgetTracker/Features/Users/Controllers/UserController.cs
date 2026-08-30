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
        [HttpPost("get-profile")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _readUserService.GetUserAsync(userId.Value);

            if (!response.Success)
            {
                return response.ErrorType switch
                {
                    UserErrorType.UserNotFound => Unauthorized(response),
                    _ => BadRequest(response)
                };
            }
            return Ok(response);
        }

        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPost("update-profile/{userId}")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var response = await _writeUserService.UpdateUserAsync(userId.Value, request);

            if (!response.Success)
            {
                return response.ErrorType switch
                {
                    UserErrorType.UserNotFound => Unauthorized(response),
                    _ => BadRequest(response)
                };
            }
            return Ok(response);
        }


        [Authorize]
        [EnableRateLimiting("General")]
        [HttpPost("delete-profile/{userId}")]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }



            var response = await _writeUserService.DeleteUserAsync(userId.Value);

            if (!response.Success)
            {
                return response.ErrorType switch
                {
                    UserErrorType.UserNotFound => Unauthorized(response),
                    _ => BadRequest(response)
                };
            }
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
