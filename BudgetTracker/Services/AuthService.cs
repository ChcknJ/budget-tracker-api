using BudgetTracker.Configs;
using BudgetTracker.Database;
using BudgetTracker.DTO;
using BudgetTracker.Interfaces;
using BudgetTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService (AppDbContext context, IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            _context = context;
        }


        public async Task<LoginResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            // chech if account already existing
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerRequest.Username);

            // if user is not null means user already has account
            if (checkUser != null)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "User already existed",
                };
            }

            // hash password
            string hashPass = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            // create user
            var user = new User
            {
                Username = registerRequest.Username,
                HashPassword = hashPass
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Generate token to make user authorize
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
            string token = GenerateJwtToken(user, expiresAt);

            return new LoginResponse
            {
                Success = true,
                Token = token,
                ExpiresAt = expiresAt
            };
        }  

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            // look for the user if it is in the db
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            // Check if user has account registered
            if (user == null)
            {
                return new LoginResponse
                { 
                    Success = false,
                    ErrorMessage = "No user found!"
                };
            }

            // Check if user password is correct
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.HashPassword))
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Incorrect Password"
                };
            }

            DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
            // Log in the user
            return new LoginResponse
            {
                Success = true,
                Token = GenerateJwtToken(user, expiresAt),
                ExpiresAt = expiresAt
            };
        }

        private string GenerateJwtToken(User user, DateTime expiresAt)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generating Token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
