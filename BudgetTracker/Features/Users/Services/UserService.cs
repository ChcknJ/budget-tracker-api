using BudgetTracker.Database;
using BudgetTracker.Features.Users.DTOs;
using BudgetTracker.Features.Users.Interfaces;
using BudgetTracker.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Features.Users.Services
{
    public class UserService : IReadUserService, IWriteUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context) { _context = context; }


        // READ USER

        public async Task<UserResult> GetUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new UserResult(
                    Success: false,
                    ErrorType: UserErrorType.UserNotFound,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            return new UserResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new UserResponse(
                    Username: user.Username,
                    EmailAddress: user.EmailAddress));
        }


        // WRITE USER
        public async Task<UserResult> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new UserResult(
                    Success: false,
                    ErrorType: UserErrorType.UserNotFound,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var updatedUser = new User
            {
                Id = user.Id,
                Username = request.Username,
                EmailAddress = request.EmailAddress,
                PasswordHash = hashPassword
            };

            await _context.SaveChangesAsync();

            return new UserResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new UserResponse(
                    Username: updatedUser.Username,
                    EmailAddress: updatedUser.EmailAddress));
        }


        public async Task<UserResult> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new UserResult(
                    Success: false,
                    ErrorType: UserErrorType.UserNotFound,
                    ErrorMessage: "Something went wrong.",
                    Response: null);
            }

            user.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new UserResult(
                    Success: true,
                    ErrorType: null,
                    ErrorMessage: null,
                    Response: null);
        }
    }
}
