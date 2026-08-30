using BudgetTracker.Database;
using BudgetTracker.Database.Configs;
using BudgetTracker.Features.Auth.DTOs;
using BudgetTracker.Features.Auth.Interfaces;
using BudgetTracker.Features.Auth.Models;
using BudgetTracker.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BudgetTracker.Features.Auth.Services
{
    public class AuthService : IWriteAuthServices
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService (AppDbContext context, IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            _context = context;
        }



        public async Task<AuthResult> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            // Check for email duplicate
            var emailDuplicates = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == registerRequest.EmailAddress, cancellationToken);

            if (emailDuplicates != null)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.InvalidEmailAddress,
                    ErrorMessage: "Email Address is already in use.",
                    Response: null);
            }

            
            string passHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            DateTime createdAt = DateTime.UtcNow;

            // Create and save User to db
            var user = new User
            {
                Username = registerRequest.Username,
                PasswordHash = passHash,
                EmailAddress = registerRequest.EmailAddress,
                CreatedAt = createdAt
            };
            await _context.Users.AddAsync(user, cancellationToken);
            return await IssueTokensAsync(user, cancellationToken);
        }  


        public async Task<AuthResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            // Check if there is account connected to email
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == loginRequest.EmailAddress, cancellationToken);
            if (existingUser == null)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.InvalidEmailAddress,
                    ErrorMessage: "Email or password is incorrect.",
                    Response: null);
            }



            // Check if user password is correct
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, existingUser.PasswordHash))
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Email or password is incorrect.",
                    Response: null);
            }

            return await IssueTokensAsync(existingUser, cancellationToken);
        }


        public async Task<AuthResult> RefreshAsync(RefreshRequest refreshRequest, CancellationToken cancellationToken)
        {
            // Convert input string to byte and hash
            string tokenHash;
            try
            {
                byte[] bytes = Convert.FromBase64String(refreshRequest.RefreshToken);
                tokenHash = Convert.ToBase64String(SHA256.HashData(bytes));
            }
            catch (Exception)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Invalid credentials.",
                    Response: null);
            }

            // Compare hashed input string to the db.
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);
            if (existingToken == null)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }


            // Check if token is not expired.
            DateTime dateNow = DateTime.UtcNow;
            if (dateNow > existingToken.ExpiresAt)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }


            // Check if token has not been used twice.
            if (existingToken.RevokedAt != null)
            {
                // Revoke all non revoked tokens of the user.
                var activeTokens = await _context.RefreshTokens
                    .Where(t => t.UserId == existingToken.UserId && t.RevokedAt == null)
                    .ToListAsync(cancellationToken);

                foreach (var t in activeTokens)
                {
                    t.RevokedAt = dateNow;
                }
                await _context.SaveChangesAsync(cancellationToken);


                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
                
            }



            // Issue a new refresh token

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == existingToken.UserId, cancellationToken);
            if (user == null)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }
            // Update the existing token
            existingToken.RevokedAt = dateNow;

            // Generate JWT
            string accessToken = GenerateJwtToken(user, dateNow.AddMinutes(_jwtSettings.ExpiryMinutes));

            // Generate new refresh Token
            var (refreshTokenPlain, newToken) = await GenerateRefreshToken(user, dateNow.AddDays(_jwtSettings.RefreshTokenExpiryDays), cancellationToken);
            // Connect the old token to new one.
            existingToken.ReplacedBy = newToken;
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResult(
                    Success: true,
                    ErrorType: null,
                    ErrorMessage: null,
                    Response: new AuthResponse(
                        AccessToken: accessToken,
                        RefreshToken: refreshTokenPlain));
        }


        public async Task<AuthResult> LogoutAsync(RefreshRequest refreshRequest, CancellationToken cancellationToken)
        {
            // Convert input string to byte and hash
            string tokenHash;
            try
            {
                byte[] bytes = Convert.FromBase64String(refreshRequest.RefreshToken);
                tokenHash = Convert.ToBase64String(SHA256.HashData(bytes));
            }
            catch (Exception)
            {
                return new AuthResult(
                    Success: false,
                    ErrorType: AuthErrorType.IncorrectCredentials,
                    ErrorMessage: "Invalid credentials.",
                    Response: null);
            }

            // Check the token and revoke it making it unusable.
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);
            if (existingToken != null)
            {
                existingToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new AuthResult(
                Success: true, 
                ErrorType: null, 
                ErrorMessage: null, 
                Response: null);
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

        private async Task<(string PlainToken, RefreshToken Entity)> GenerateRefreshToken(User user, DateTime expiresAt, CancellationToken cancellationToken)
        {
            // Generates random string
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);


            // Hash string
            string tokenHash = Convert.ToBase64String(SHA256.HashData(randomBytes));

            // Create refresh token model
            var refreshToken = new RefreshToken
            {
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                User = user
            };

            // Save to db
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);

            return (Convert.ToBase64String(randomBytes), refreshToken);
        }

        private async Task<AuthResult> IssueTokensAsync(User user, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            string accessToken = GenerateJwtToken(user, now.AddMinutes(_jwtSettings.ExpiryMinutes));
            var (refreshTokenPlain, _) = await GenerateRefreshToken(user, now.AddDays(_jwtSettings.RefreshTokenExpiryDays), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new AuthResponse(AccessToken: accessToken, RefreshToken: refreshTokenPlain));
        }

    }
}
