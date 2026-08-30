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


        // !!!!! TODO: email, pass, username check in updateuser



        // READ USER

        public async Task<UserResult> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                return new UserResult(
                    Success: false,
                    ErrorType: UserErrorType.UserNotFound,
                    ErrorMessage: "User cannot be found.",
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
        public async Task<UserResult> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                return new UserResult(
                    Success: false,
                    ErrorType: UserErrorType.UserNotFound,
                    ErrorMessage: "User cannot be found.",
                    Response: null);
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Username = request.Username;
            user.EmailAddress = request.EmailAddress;
            user.PasswordHash = hashPassword;

            await _context.SaveChangesAsync(cancellationToken);

            return new UserResult(
                Success: true,
                ErrorType: null,
                ErrorMessage: null,
                Response: new UserResponse(
                    Username: user.Username,
                    EmailAddress: user.EmailAddress));
        }


        public async Task<UserResult> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                return new UserResult(
                    Success: false,
                    ErrorType: UserErrorType.UserNotFound,
                    ErrorMessage: "User cannot be found.",
                    Response: null);
            }

            user.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return new UserResult(
                    Success: true,
                    ErrorType: null,
                    ErrorMessage: null,
                    Response: null);
        }
    }
}
