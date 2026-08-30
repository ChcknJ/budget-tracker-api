using BudgetTracker.Features.Auth.DTOs;
using BudgetTracker.Features.Auth.Interfaces;
using BudgetTracker.Features.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BudgetTracker.Features.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWriteAuthServices _authService;


        public AuthController(IWriteAuthServices authService)
        {
            _authService = authService;
        }



        [EnableRateLimiting("Auth")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellation)
        {
            var response = await _authService.RegisterAsync(request, cancellation);

            if (!response.Success)
            {
                return response.ErrorType switch
                {
                    AuthErrorType.InvalidEmailAddress => Conflict(response),
                    AuthErrorType.IncorrectCredentials => Unauthorized(response),
                    AuthErrorType.UserCannotBeFound => NotFound(response),
                    _ => BadRequest(response)
                };
            }

            return Created(string.Empty, response);
        }


        [EnableRateLimiting("Auth")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(request, cancellationToken);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        [EnableRateLimiting("Auth")]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.RefreshAsync(request, cancellationToken);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(RefreshRequest request, CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(request, cancellationToken);
            return Ok();
        }


    }
}
